using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ApplicationCore.Repository
{
    public interface IRepository<T> where T:class
    {
        public IQueryable<T> GetItems();
        public IQueryable<T> GetItems(Expression<Func<T, bool>> condition);
        public T GetItem(object Id);

        public void AddItem(T Entity);
        public void AddItems(IEnumerable<T> entities);
    }
}
