using CMS.ApplicationCore.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> _dbSet;

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public DbContext DbContext { get; private set; }

        public void AddItem(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddItems(IEnumerable<T> entities)
        {
            _dbSet.AddRangeAsync(entities);
        }

        public T GetItem(object id)
        {
            return _dbSet.Find(id);
        }

        public IQueryable<T> GetItems()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetItems(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Where(condition);
        }

        public void RemoveItem(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveItems(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void UpdateItem(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateItems(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }
}
