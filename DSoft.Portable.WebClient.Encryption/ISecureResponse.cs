using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public interface ISecureResponse<T> where T : ISecurePayload
    {
        T Payload { get; set; }

        void SetPayLoad(object data, string passKey);

        void SetPayLoad(string data);

        TData Extract<TData>(string passKey);
    }
}
