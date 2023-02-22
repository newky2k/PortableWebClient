using DSoft.Portable.Server.Security.Core;
using Microsoft.EntityFrameworkCore;

namespace DSoft.Portable.EntityFrameworkCore.Security
{
	/// <summary>
	/// Interface ISessionDataContext
	/// Extends the <see cref="IDbContext" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TKey">The type of the t key.</typeparam>
	/// <typeparam name="TTokenType">The type of the t token type.</typeparam>
	/// <seealso cref="IDbContext" />
	public interface ISessionDataContext<T, TKey, TTokenType> : IDbContext
    where T : class, ISession<TKey, TTokenType>, new()
    {
		/// <summary>
		/// Gets or sets the sessions.
		/// </summary>
		/// <value>The sessions.</value>
		DbSet<T> Sessions { get; set; }

    }
}
