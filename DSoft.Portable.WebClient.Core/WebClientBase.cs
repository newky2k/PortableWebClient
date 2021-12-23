using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace DSoft.Portable.WebClient
{
    /// <summary>
    /// Base Web Client class for connecting to an Web API application
    /// </summary>
    public abstract class WebClientBase : IWebClient
    {
        #region Fields
        private Version _appVersion;
        private string _baseUrl;
        private int _defaultTimeOutSeconds = 0; //same as RestClient default

        #endregion

        #region Properties

        /// <summary>
        /// Gets the base Url for the connection
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl => _baseUrl;

        /// <summary>
        /// Returns the Url to be added to the base url for connection testing.  
        /// </summary>
        protected virtual string ConnectionTestSubUrl => string.Empty;

        private string TestUrl => $"{BaseUrl}/{ConnectionTestSubUrl}";

        /// <summary>
        /// Gets or sets the time out
        /// </summary>
        /// <value>
        /// The time out.
        /// </value>
        public int TimeOut { get => _defaultTimeOutSeconds; set => _defaultTimeOutSeconds = value; }

        /// <summary>
        /// Gets a value indicating whether this instance can connect to the server
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can connect; otherwise, <c>false</c>.
        /// </value>
        public bool CanConnect => CheckCanConnect(TimeOut);

        /// <summary>
        /// Gets the client version no, from the assemblt
        /// </summary>
        /// <value>
        /// The client version no.
        /// </value>
        public string ClientVersionNo
        {
            get
            {
                if (_appVersion == null)
                {
                    var asm = Assembly.GetAssembly(this.GetType());

                    _appVersion = asm.GetName().Version;
                }


                return _appVersion.ToString();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientBase"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public WebClientBase(string baseUrl)
        {
            _baseUrl = baseUrl;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks that the connection can be made, asyncronously
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public async Task<bool> CheckCanConnectAsync(int timeout = 1, bool throwException = false)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(timeout);

                var thing = await client.GetAsync(TestUrl);

                if (thing.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    if (throwException == true)
                        throw new Exception("Not found");

                    return false;
                }

                return true;
            }
            catch (WebException)
            {
                if (throwException == true)
                    throw;

                // handle web exception
                return false;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
            catch (Exception)
            {
                if (throwException == true)
                    throw;

                return false;
            }

        }

        /// <summary>
        /// Checks that the connection can be made
        /// </summary>
        /// <returns></returns>
        public bool CheckCanConnect(int timeout = 1, bool throwException = false) => CheckCanConnectAsync(timeout, throwException).GetAwaiter().GetResult();
       

        public virtual void Dispose()
        {
           
        }

        /// <summary>
        /// Get an instance of a service that implements ServiceClientBase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Service<T>() where T : IWebClient
        {
            return (T)Activator.CreateInstance(typeof(T), this);
        }

        #endregion
    }

}
