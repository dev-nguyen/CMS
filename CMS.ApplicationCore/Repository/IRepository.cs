using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.ApplicationCore
{
    public interface IRepository<T> where T:class
    {
        public IQueryable<T> GetItems();
        public IQueryable<T> GetItems(int currentPage, int pageSize, Expression<Func<T, bool>> condition = null);
        public T GetItem(object Id);
        public int Count(Expression<Func<T, bool>> condition = null);

        public void AddItem(T entity);
        public void AddItems(IEnumerable<T> entities);

        public void UpdateItem(T entity);
        public void UpdateItems(IEnumerable<T> entities);

        public T RemoveItem(T entity);
        public void RemoveItems(IEnumerable<T> entities);

        //public DbContext DbContext { get; }

    }
}
