using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public interface ISecurePayload
    {
        string Data { get; set; }

        bool Validate(TimeSpan timeSpan);

        T Extract<T>(string passKey, string initVector, KeySize keySize = KeySize.TwoFiftySix);
    }
}
