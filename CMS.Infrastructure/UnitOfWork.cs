using CMS.ApplicationCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CMS.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CMSContext _context;

        public UnitOfWork(CMSContext context)
        {
            _context = context;
        }
        //public DbContext DbContext { 
        //    get {
        //        return _context;
        //    }
        //}
        public void Commit()
        {
            _context.SaveChanges();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        public void Transaction(Action action)
        {
            action.Invoke();
        }
    }
}
