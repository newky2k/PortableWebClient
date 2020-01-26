using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public class SecureRequest : RequestBase
    {
        public string Id { get; set; }

        public SecurePayload Payload { get; set; }

        public SecureRequest()
        {
            Payload = new SecurePayload();
        }

        public SecureRequest(string passKey)
        {
            Payload = new SecurePayload(passKey);
        }

        public SecureRequest(string passKey, object data)
        {
            Payload = new SecurePayload(data, passKey);
        }

        public SecureRequest(string passKey, DateTime timestamp) : this(passKey)
        {
            Payload.Timestamp = timestamp;
        }

        public SecureRequest(string passKey, string clientVersionNumber) : this(passKey)
        {
            ClientVersionNo = clientVersionNumber;
        }

        public SecureRequest(string passKey, string clientVersionNumber, DateTime timestamp) : this(passKey, clientVersionNumber)
        {
            Payload.Timestamp = timestamp;
        }

        public void Validate(TimeSpan timeout)
        {
            if (Payload == null)
                throw new Exception("No payload in request");

            if (!Payload.Validate(timeout))
                throw new Exception("Payload has timed out");
        }
    }
}
