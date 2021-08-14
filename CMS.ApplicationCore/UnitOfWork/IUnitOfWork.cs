using System;

namespace CMS.ApplicationCore
{
    public interface IUnitOfWork
    {
        public IRepository<T> GetRepository<T>() where T:class;
        public void Commit();

        public void Transaction(Action action);

    }
}
