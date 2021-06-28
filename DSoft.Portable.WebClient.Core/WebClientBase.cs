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
        private int _defaultTimeOutSeconds = 1;

        #endregion

        #region Properties

        public string BaseUrl => _baseUrl;

        public int TimeOut { get => _defaultTimeOutSeconds; set => _defaultTimeOutSeconds = value; }

        public bool CanConnect => CheckCanConnect(TimeOut);

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

                var thing = await client.GetAsync(BaseUrl);

                if (thing.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

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
        public bool CheckCanConnect(int timeout = 1, bool throwException = false)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(timeout);

                var thing = client.GetAsync(BaseUrl).Result;

                if (thing.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

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
                if (throwException == true)
                    throw;

                return false;
            }
            catch (Exception)
            {
                if (throwException == true)
                    throw;

                return false;
            }

        }

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
