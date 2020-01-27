using DSoft.Portable.Server.Security.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DSoft.Portable.EntityFrameworkCore.Security
{
    public abstract class SessionManagerBase<T, TDbContext, TKey, TTokenType> : IDisposable
    where T : class, ISession<TKey, TTokenType>, new()
    where TDbContext : class, ISessionDataContext<T, TKey, TTokenType>
    {

        private TDbContext _dbContext;

        public SessionManagerBase(TDbContext dataContext)
        {
            _dbContext = dataContext;

        }

        /// <summary>
        /// Create a new session asyncronously
        /// </summary>
        /// <returns>new session of type</returns>
        public async Task<T> CreateSessionAsync()
        {
            var newSession = new T()
            {
                // Token = TokenHelper.GenerateToken(),
            };

            await _dbContext.Sessions.AddAsync(newSession);
            await _dbContext.SaveChangesAsync();

            return newSession;
        }

        /// <summary>
        /// Find the specific session asyncronously
        /// </summary>
        /// <param name="sessionId">Id of the session</param>
        /// <returns></returns>
        public async Task<T> FindSessionAsync(TKey sessionId)
        {
            var session = await _dbContext.Sessions.FirstOrDefaultAsync(x => x.Id.Equals(sessionId));

            if (session == null)
                throw new Exception("Session id is invalid");

            return session;
        }

        /// <summary>
        /// Delete the specific session asyncronously
        /// </summary>
        /// <param name="sessionId">Id of the session</param>
        /// <returns></returns>
        public async Task DeleteAsync(TKey sessionId)
        {
            var session = await _dbContext.Sessions.FirstOrDefaultAsync(x => x.Id.Equals(sessionId));

            if (session == null)
                return;

            _dbContext.Sessions.Remove(session);
            await _dbContext.SaveChangesAsync();

        }

        /// <summary>
        /// Remove Expired sessions asyncronously
        /// </summary>
        /// <returns></returns>
        public async Task RemoveExpiredSessionsAsync()
        {
            var sessions = await _dbContext.Sessions.Where(x => x.Expires < DateTime.Now).ToListAsync();

            _dbContext.Sessions.RemoveRange(sessions);
            await _dbContext.SaveChangesAsync();
        }

		public void Dispose()
		{
            _dbContext = null;
		}
	}
}
