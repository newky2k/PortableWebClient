using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.Portable.Server.Security.Core
{
    public interface ISession<T, T2>
    {
        T Id { get; set; }

        T2 Token { get; set; }

        DateTime Timestamp { get; set; }

        DateTime Expires { get; set; }
    }
}
