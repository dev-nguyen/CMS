using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.ApplicationCore.Repository
{
    public interface IRepository<T> where T:class
    {
        public IQueryable<T> GetItems();
        public IQueryable<T> GetItems(Expression<Func<T, bool>> condition);
        public T GetItem(object Id);

        public void AddItem(T entity);
        public void AddItems(IEnumerable<T> entities);

        public void UpdateItem(T entity);
        public void UpdateItems(IEnumerable<T> entities);

        public void RemoveItem(T entity);
        public void RemoveItems(IEnumerable<T> entities);
    }
}
