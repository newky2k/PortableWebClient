using DSoft.Portable.WebClient.Encryption;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Rest.Encryption
{
    public class SecureBinaryResponse : SecureResponse, ISecureBinaryResponse<SecurePayload, byte[]>
    {
        public string FileName { get; set; }

        public string MimeType { get; set; }

        public byte[] Data { get; set; }

        public SecureBinaryResponse()
        {

        }
    }
}
