using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient.Encryption
{
    public interface ISecureRequest<T> where T : ISecurePayload
    {
        string Id { get; set; }

        string TokenId { get; set; }

        T Payload { get; set; }

        void Validate(TimeSpan timeout);

        TData ExtractPayload<TData>(string passKey);
    }
}
