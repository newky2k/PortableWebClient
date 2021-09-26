using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public interface ISecureResponse<T> where T : ISecurePayload
    {
        T Payload { get; set; }

        void SetPayload(object data, string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);

        void SetPayload(string data);

        TData Extract<TData>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
    }
}
