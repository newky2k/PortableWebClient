using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public class SecureResponse : ResponseBase
    {
        public SecurePayload Payload { get; set; }

        public SecureResponse()
        {
            Success = true;

            Payload = new SecurePayload();
        }
    }
}
