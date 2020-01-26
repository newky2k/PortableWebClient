using DSoft.Portable.Server.Security.Core;
using Microsoft.EntityFrameworkCore;

namespace DSoft.EntityFrameworkCore.Security
{
    public interface ISessionDataContext<T, TKey, TTokenType> : IDbContext
    where T : class, ISession<TKey, TTokenType>, new()
    {
        DbSet<T> Sessions { get; set; }

    }
}
