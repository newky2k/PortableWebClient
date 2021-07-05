using DSoft.Portable.WebClient.Encryption;
using DSoft.Portable.WebClient.Encryption.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
    public class SecureResponse : ResponseBase, ISecureResponse<SecurePayload>
    {
        public SecurePayload Payload { get; set; }

        public SecureResponse()
        {
            Success = true;

            Payload = new SecurePayload();
        }

        public void SetPayload(string data)
        {
            if (Payload == null)
                throw new Exception("Payload is not set");

            Payload.Data = data;
        }

        public void SetPayload(object data, string passKey) => SetPayload(PayloadManager.EncryptPayload(data, passKey));

        public TData Extract<TData>(string passKey)
        {
            if (Payload == null)
                throw new Exception("No data");

            return Payload.Extract<TData>(passKey);
        }

        
    }
}
