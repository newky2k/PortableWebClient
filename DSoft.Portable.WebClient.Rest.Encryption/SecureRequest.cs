using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
    public class SecureRequest : RequestBase, ISecureRequest<SecurePayload>
    {
        public string Id { get; set; }

        public string TokenId { get; set; }

        public SecurePayload Payload { get; set; }

        public string Context { get; set; }

        public SecureRequest()
        {
            Payload = new SecurePayload();
        }

        public SecureRequest(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            Payload = new SecurePayload(passKey, initVector, keySize);
        }

        public SecureRequest(string passKey, object data, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            Payload = new SecurePayload(data, passKey, initVector, keySize);
        }

        public SecureRequest(string passKey, DateTime timestamp, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, initVector, keySize)
        {
            Payload.Timestamp = timestamp;
        }

        public SecureRequest(string passKey, string clientVersionNumber, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, initVector, keySize)
        {
            ClientVersionNo = clientVersionNumber;
        }

        public SecureRequest(string passKey, string clientVersionNumber, DateTime timestamp, string initVector, KeySize keySize = KeySize.TwoFiftySix) : this(passKey, clientVersionNumber, initVector, keySize)
        {
            Payload.Timestamp = timestamp;
        }

        public void Validate(TimeSpan timeout)
        {
            if (Payload == null)
                throw new Exception("No payload in request");

            if (Payload.Timestamp == default || !Payload.Validate(timeout))
                throw new Exception("Payload has timed out");
        }

        public TData ExtractPayload<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix)
        {
            if (Payload == null)
                throw new Exception("No data");

            return Payload.Extract<TData>(passKey, initVector, keySize);
        }

    }
}
