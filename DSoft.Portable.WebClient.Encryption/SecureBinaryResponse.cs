using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public class SecureBinaryResponse : SecureResponse
    {
        public string FileName { get; set; }

        public string MimeType { get; set; }

        public byte[] Data { get; set; }

        public SecureBinaryResponse()
        {

        }
    }
}
