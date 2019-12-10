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

        public void Validate(TimeSpan timeout)
        {
            if (Payload == null)
                throw new Exception("No payload in request");

            if (!Payload.Validate(timeout))
                throw new Exception("Payload has timed out");
        }
    }
}
