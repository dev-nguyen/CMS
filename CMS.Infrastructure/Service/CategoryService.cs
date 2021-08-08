using CMS.ApplicationCore.Service;
using CMS.ApplicationCore.UnitOfWork;
using CMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Infrastructure.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWOrk)
        {
            _unitOfWork = unitOfWOrk;
        }
        public void CreateCatalog(Category category)
        {
            var categoryRepository = _unitOfWork.GetRepository<Category>();

        }
    }
}
