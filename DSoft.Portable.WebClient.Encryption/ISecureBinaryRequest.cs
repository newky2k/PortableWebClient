using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public interface ISecureBinaryRequest<T, T2> : ISecureRequest<T> where T : ISecurePayload
    {
        T2 BinaryObject { get; set; }

        void SetBinaryObject(byte[] data, string passKey);

        byte[] GetBinaryObject(string passKey);
    }
}
