using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public interface ISecurePayload
    {
        bool Validate(TimeSpan timeSpan);

        T Extract<T>(string passKey);
    }
}
