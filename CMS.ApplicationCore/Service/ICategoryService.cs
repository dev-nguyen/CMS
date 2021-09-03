using CMS.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CMS.ApplicationCore.Service
{
    public interface ICategoryService
    {
        public Category CreateCatalog(Category category);
        public IQueryable<Category> GetCategories(int currentPage, int Pagesize);
        public int Count(Expression<Func<Category, bool>> condition = null);

        public Category UpdateCategory(Category category);
        public Category GetCategoryById(Guid id);

        public Category DeleteCategory(string[] ids);

    }
}
