using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
    public class SecurePayload : ISecurePayload
    {
        #region Properties

        public DateTime Timestamp { get; set; }

        public string Data { get; set; }


        #endregion

        #region Constructors

        public SecurePayload()
        {
            Timestamp = DateTime.Now;
        }

        public SecurePayload(object dataValue, string passKey) : this()
        {
            Data = PayloadManager.EncryptPayload(dataValue, passKey);
        }

        public SecurePayload(string passKey) : this(EmptyPayLoad.Empty, passKey)
        {

        }

        #endregion

        #region Methods

        public bool Validate(TimeSpan timeSpan)
        {
            var diff = DateTime.Now - Timestamp;

            return (diff < timeSpan);
        }

        public T Extract<T>(string passKey)
        {
            return PayloadManager.DecryptPayload<T>(Data, passKey);
        }

        #endregion
    }

    
}
