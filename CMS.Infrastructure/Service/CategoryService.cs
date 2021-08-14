using CMS.ApplicationCore;
using CMS.ApplicationCore.Service;
using CMS.Entity;

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
