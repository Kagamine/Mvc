// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Infrastructure;

namespace Microsoft.AspNet.Mvc.Routing
{
    /// <summary>
    /// Identifies an action that only supports a given set of HTTP methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class HttpMethodAttribute : Attribute, IActionHttpMethodProvider, IRouteTemplateProvider
    {
        private readonly IEnumerable<string> _httpMethods;
        private int? _order;

        /// <summary>
        /// Creates a new <see cref="HttpMethodAttribute"/> with the given
        /// set of HTTP methods.
        /// <param name="httpMethods">The set of supported HTTP methods.</param>
        /// </summary>
        public HttpMethodAttribute(IEnumerable<string> httpMethods)
            : this(httpMethods, null)
        {
            if (httpMethods == null)
            {
                throw new ArgumentNullException(nameof(httpMethods));
            }
        }

        /// <summary>
        /// Creates a new <see cref="HttpMethodAttribute"/> with the given
        /// set of HTTP methods an the given route template.
        /// </summary>
        /// <param name="httpMethods">The set of supported methods.</param>
        /// <param name="template">The route template. May not be null.</param>
        public HttpMethodAttribute(IEnumerable<string> httpMethods, string template)
        {
            if (httpMethods == null)
            {
                throw new ArgumentNullException(nameof(httpMethods));
            }

            _httpMethods = httpMethods;
            Template = template;
        }

        /// <inheritdoc />
        public IEnumerable<string> HttpMethods
        {
            get
            {
                return _httpMethods;
            }
        }

        /// <inheritdoc />
        public string Template { get; private set; }

        /// <summary>
        /// Gets the route order. The order determines the order of route execution. Routes with a lower
        /// order value are tried first. When a route doesn't specify a value, it gets the value of the
        /// <see cref="RouteAttribute.Order"/> or a default value of 0 if the <see cref="RouteAttribute"/>
        /// doesn't define a value on the controller.
        /// </summary>
        public int Order
        {
            get { return _order ?? 0; }
            set { _order = value; }
        }

        /// <inheritdoc />
        int? IRouteTemplateProvider.Order
        {
            get
            {
                return _order;
            }
        }

        /// <inheritdoc />
        public string Name { get; set; }
    }
}