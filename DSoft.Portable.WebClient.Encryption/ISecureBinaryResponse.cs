using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public interface ISecureBinaryResponse<T,T2> : ISecureResponse<T> where T : ISecurePayload
    {
        string FileName { get; set; }

        string MimeType { get; set; }

        T2 Data { get; set; }
    }
}
