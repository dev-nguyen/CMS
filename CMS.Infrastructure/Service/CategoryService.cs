using CMS.ApplicationCore;
using CMS.ApplicationCore.Service;
using CMS.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CMS.Infrastructure.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWOrk)
        {
            _unitOfWork = unitOfWOrk;
            _categoryRepository = _unitOfWork.GetRepository<Category>();
        }



        [HttpPost]
        public Category CreateCatalog([FromBody]Category category)
        {
            _categoryRepository.AddItem(category);
            _unitOfWork.Commit();
            return category;
        }

        public IQueryable<Category> GetCategories(int currentPage, int Pagesize)
        {
            return _categoryRepository.GetItems(currentPage, Pagesize);
        }
        public int Count(Expression<Func<Category, bool>> condition = null)
        {
            return _categoryRepository.Count(condition);
        }

        public Category UpdateCategory(Category category)
        {
            _categoryRepository.UpdateItem(category);
            _unitOfWork.Commit();

            return category;
        }
        public Category GetCategoryById(Guid id)
        {
            return _categoryRepository.GetItem(id);
        }

        public Category  DeleteCategory(string[] ids) {
            Category category = null;
            if (ids.Count() == 1)
            {
                Guid id = Guid.Parse(ids[0]);
                var item = _categoryRepository.GetItem(id);
                category = _categoryRepository.RemoveItem(item);
            }
            else
            {
                List<Category> items = new();
                foreach (string id in ids)
                {
                    var item = _categoryRepository.GetItem(Guid.Parse(id));
                    items.Add(item);
                }
                _categoryRepository.RemoveItems(items);
            }
            _unitOfWork.Commit();
            return category;
        }
    }
}
