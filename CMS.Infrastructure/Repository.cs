using CMS.ApplicationCore;
using CMS.ApplicationCore.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        public IQueryable<T> GetItems(int currentPage, int pageSize, Expression<Func<T, bool>> condition = null, List<SorterRequest> sorters = null)
        {
            //var a = Common.GetOrderByFunction<T>(sorters);
            IQueryable<T> query = _dbSet;
            if (condition != null)
                query = query.Where(condition);
            //query = (sorters != null) ? Common.CreateSortingExpression<T>(sorters, query) : query;
            query = query.OrderBy(sorters);
            return query.Skip(currentPage).Take(pageSize);
        }

        public int Count(Expression<Func<T, bool>> condition = null)
        {
            IQueryable<T> query = _dbSet;
            if (condition != null)
                query = query.Where(condition);

            return query.Count();
        }

        public T RemoveItem(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }

        public void RemoveItems(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void UpdateItem(T entity)
        {
            //DbContext.Attach(entity);
            //DbContext.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
        }

        public void UpdateItems(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }
}
