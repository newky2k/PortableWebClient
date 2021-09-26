using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.WebClient
{
    public interface IWebClient : IDisposable
    {
        string BaseUrl { get; }

        string ClientVersionNo { get; }

        bool CanConnect { get; }

        int TimeOut { get; }

    }
}
