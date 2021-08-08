using CMS.ApplicationCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ApplicationCore.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRepository<T> GetRepository<T>() where T:class;
        public void Commit();

        public void Transaction(Action action);

    }
}
