using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tasks.Database
{
    public class EfGenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public EfGenericRepository(DbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        #region IGenericRepository<T> implementation

        public Task<IQueryable<T>> AsQueryableAsync()
        {
            return Task.Run(() => _dbSet.AsQueryable());
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.Run(() => _dbSet.AsEnumerable());
        }

        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.Run(() => _dbSet.Where(predicate).AsEnumerable());
        }

        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string[] includes)
        {
            return Task.Run(() =>
            {
                foreach (var include in includes)
                {
                    _dbSet.Include(include);
                }

                return _dbSet.Where(predicate).AsEnumerable();
            });
        }

        public Task<int> CountAsync()
        {
            return _dbSet.CountAsync();
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.SingleAsync(predicate);
        }

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.SingleOrDefaultAsync(predicate);
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstAsync(predicate);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefaultAsync(predicate);
        }

        public Task<T> GetByIdAsync(int id)
        {
            return _dbSet.FindAsync(id);
        }

        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dbSet.Add(entity);
        }

        public Task AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return Task.Run(() =>
            {
                _dbSet.Add(entity);
            });
        }

        public Task DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return Task.Run(() =>
            {
                _dbSet.Remove(entity);
            });
        }

        public Task AttachAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return Task.Run(() =>
            {
                _dbSet.Attach(entity);
            });
        }

        #endregion
    }
}
