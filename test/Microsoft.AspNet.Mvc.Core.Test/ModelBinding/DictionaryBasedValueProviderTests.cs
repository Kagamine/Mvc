// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    public class DictionaryBasedValueProviderTests
    {
        [Fact]
        public void GetValueProvider_ReturnsNull_WhenKeyIsNotFound()
        {
            // Arrange
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { "test-key", "value" }
            };
            var provider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            // Act
            var result = provider.GetValue("not-test-key");

            // Assert
            Assert.Equal(ValueProviderResult.None, result);
        }

        [Fact]
        public void GetValueProvider_ReturnsValue_IfKeyIsPresent()
        {
            // Arrange
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { "test-key", "test-value" }
            };
            var provider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            // Act
            var result = provider.GetValue("test-key");

            // Assert
            Assert.Equal("test-value", (string)result);
        }

        [Fact]
        public void ContainsPrefix_ReturnsNullValue_IfKeyIsPresent()
        {
            // Arrange
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { "test-key", null }
            };
            var provider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            // Act
            var result = provider.GetValue("test-key");

            // Assert
            Assert.Equal(string.Empty, (string)result);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        [InlineData("bar.baz")]
        public void ContainsPrefix_ReturnsTrue_ForKnownPrefixes(string prefix)
        {
            // Arrange
            var values = new Dictionary<string, object>
            {
                { "foo", 1 },
                { "bar.baz", 1 },
            };

            var valueProvider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            // Act
            var result = valueProvider.ContainsPrefix(prefix);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("bar", "1")]
        [InlineData("bar.baz", "2")]
        public void GetValue_ReturnsCorrectValue_ForKnownKeys(string prefix, string expectedValue)
        {
            // Arrange
            var values = new Dictionary<string, object>
            {
                { "bar", 1 },
                { "bar.baz", 2 },
            };

            var valueProvider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            // Act
            var result = valueProvider.GetValue(prefix);

            // Assert
            Assert.Equal(expectedValue, (string)result);
        }

        [Fact]
        public void GetValue_DoesNotReturnAValue_ForAKeyPrefix()
        {
            // Arrange
            var values = new Dictionary<string, object>
            {
                { "bar.baz", 2 },
            };

            var valueProvider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            // Act
            var result = valueProvider.GetValue("bar");

            // Assert
            Assert.Equal(ValueProviderResult.None, result);
        }

        [Fact]
        public void ContainsPrefix_ReturnsFalse_IfKeyIsNotPresent()
        {
            // Arrange
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { "test-key", "test-value" }
            };
            var provider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            // Act
            var result = provider.ContainsPrefix("not-test-key");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ContainsPrefix_ReturnsTrue_IfKeyIsPresent()
        {
            // Arrange
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                { "test-key", "test-value" }
            };
            var provider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            // Act
            var result = provider.ContainsPrefix("test-key");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FilterInclude()
        {
            // Arrange
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            var provider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            var bindingSource = new BindingSource(
                BindingSource.Query.Id,
                displayName: null,
                isGreedy: true,
                isFromRequest: true);

            // Act
            var result = provider.Filter(bindingSource);

            // Assert
            Assert.NotNull(result);
            Assert.Same(result, provider);
        }

        [Fact]
        public void FilterExclude()
        {
            // Arrange
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            var provider = new DictionaryBasedValueProvider(BindingSource.Query, values);

            var bindingSource = new BindingSource(
                "Test",
                displayName: null,
                isGreedy: true,
                isFromRequest: true);

            // Act
            var result = provider.Filter(bindingSource);

            // Assert
            Assert.Null(result);
        }
    }
}
