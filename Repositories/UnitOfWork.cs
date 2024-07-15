using Core.Enitities;
using Core.Interfaces;
using Repositories;
using System.Collections;

namespace Infrastructure.Data
{
    /// <summary>
    /// Base implementation of the unit of work interface.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private Hashtable _repositories;

        /// <summary>
        /// Injects a DbContext instance to be used by all repositories.
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(DataContext context) 
        {
            _context = context;
        }

        /// <summary>
        /// The asynchronous Complete method persists changes made to the database.
        /// </summary>
        /// <returns>The task result contains the number of affected rows</returns>
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// The Dispose method implements the IDisposable interface 
        /// and ensures proper resource cleanup associated with the UnitOfWork.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }

        /// <summary>
        /// The Repository method provides a generic way to retrieve a 
        /// repository instance for a specific entity type T. 
        /// It implements a caching mechanism to optimize repository creation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// Casts the retrieved repository instance (from the cache or newly created) 
        /// to IGenericRepository of type T and returns it.
        /// </returns>
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType
                    (typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }
    }
}
