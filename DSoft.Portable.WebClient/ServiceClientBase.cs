using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DSoft.Portable.WebClient
{

    /// <summary>
    /// Base Service Client  class for consuming services provided by ASP.NET ApiControllers
    /// </summary>
    public abstract class ServiceClientBase : IDisposable
    {
        #region Fields
        private WebClientBase _client;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the rest client.
        /// </summary>
        /// <value>
        /// The rest client.
        /// </value>
        public RestClient RestClient
        {
            get
            {
                return new RestClient { BaseUrl = new Uri(_client.BaseUrl) };
            }
        }

        /// <summary>
        /// Gets the name of the Web Api Controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        public abstract string ControllerName { get; }

        /// <summary>
        /// Optional api module name if the api has been modularized  /api/module/controller
        /// </summary>
        /// <value>The module.</value>
        public virtual string Module
        {
            get
            {
                return null;
            }
        }
        #endregion



        public ServiceClientBase(WebClientBase client)
        {
            _client = client;

        }

        #region Methods

        #region Methods

        public WebClientBase WebClient
        {
            get
            {
                return _client;
            }
        }
        /// <summary>
        /// Access a typed version of the client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        protected T Client<T>() where T : WebClientBase
        {
            return (T)_client;
        }

        /// <summary>
        /// Calculates the URL for method based on BaseUrl of the Client, the controller name and the method name
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        protected string CalculateUrlForMethod(string methodName)
        {
            var url = string.Format("api/{0}/{1}", ControllerName, methodName);

            if (!string.IsNullOrWhiteSpace(Module))
            {
                url = string.Format("api/{0}/{1}/{2}", Module, ControllerName, methodName);
            }

            return url;
        }

        /// <summary>
        /// Builds a Post Request for the method 
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="type">The data format.</param>
        /// <returns></returns>
        protected RestRequest BuildPostRequest(string methodName, DataFormat type = DataFormat.Json)
        {
            return new RestRequest(CalculateUrlForMethod(methodName), Method.POST, type);
        }

        /// <summary>
        /// Builds a Get Request for the method
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="type">The data format</param>
        /// <returns></returns>
        protected RestRequest BuildGetRequest(string methodName, DataFormat type = DataFormat.Json)
        {
            return new RestRequest(CalculateUrlForMethod(methodName), Method.GET, type);
        }

        public virtual void Dispose()
        {
            _client = null;
        }

        #endregion

        #endregion
    }
}
