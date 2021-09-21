using CMS.ApplicationCore.DTO;
using CMS.ApplicationCore.Service;
using CMS.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using CMS.Infrastructure;
using System.Linq.Expressions;

namespace CMS.Web.Areas.Admin.Controllers
{
    //[Route("/Catalog")]
    //[Authorize]
    [Area("Admin")]
    //[Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        // GET: CatalogController

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public ActionResult Index()
        {
            return View();
        }

        // GET: CatalogController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CatalogController/Create
        public ActionResult Create()
        {
            return PartialView("Create");
        }

        // POST: CatalogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm]CategoryRequest request)
        {
            StatusResult status = new StatusResult();
            Category result = null;
            try
            {
                Category category = new Category();
                category.Title = request.Title;
                result = _categoryService.CreateCatalog(category);
                status.IsSuccess = true;
                status.Message = "Success";
                status.Data = result;
            }
            catch (Exception ex)
            {
                status.IsSuccess = false;
                status.Message = ex.ToString();

            }
            return Json(status);
        }

        // GET: CatalogController/Edit/5
        public ActionResult Edit(string id)
        {
            var category = _categoryService.GetCategoryById(Guid.Parse(id));
            return PartialView(category);
        }

        // POST: CatalogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromForm] CategoryRequest request)
        {
            StatusResult status = new StatusResult();
            var data = _categoryService.GetCategoryById(Guid.Parse(request.Id));
            data.Title = request.Title;

            try
            {
                var category = _categoryService.UpdateCategory(data);
                status.IsSuccess = true;
                status.Message = "Success";
                status.Data = category;
            }
            catch (Exception ex)
            {
                status.IsSuccess = false;
                status.Message = ex.ToString();
            }
            return Json(status);
        }

        // GET: CatalogController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CatalogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string ids)
        {
            StatusResult status = new StatusResult();
            try
            {
                string[] arrIds = ids.Replace("[", string.Empty).Replace("]", string.Empty).Split(",");
                status.Data = _categoryService.DeleteCategory(arrIds);
                status.IsSuccess = true;
                status.Message = "Success";
            }
            catch (Exception ex)
            {
                status.IsSuccess = false;
                status.Message = ex.ToString();
            }
            return Json(status);
        }

        
        public IActionResult Variables()
        {
            return View();
        }

        public IActionResult LoadData(int offset, int limit, string search, string sort, string order, string multiSort, string filter)
        {
            // adding sort and order into list because after chosing multiple sort, user can click header to sort
            List<SorterRequest> sorters = null;// = new List<SorterRequest>();
            List<FilterRequest> filters = null;

            if (!string.IsNullOrEmpty(multiSort))
            {
                sorters = JsonSerializer.Deserialize<List<SorterRequest>>(multiSort);
                if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
                {
                    sorters.Add(new SorterRequest() {
                        sortName = sort,
                        sortOrder = order
                    });
                }
            }
            else if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                sorters = new List<SorterRequest>();
                sorters.Add(new SorterRequest()
                {
                    sortName = sort,
                    sortOrder = order
                });
            }

            if (!string.IsNullOrEmpty(filter))
            {
                filters = JsonSerializer.Deserialize<List<FilterRequest>>(filter);
                filters.LastOrDefault().logic = "and";
                if (!string.IsNullOrEmpty(search))
                    filters.AddRange(Common.BuildValuesForFreeText<Category>(search));

            }
            //else if(!string.IsNullOrEmpty(search))
            //{
            //    filters = Common.BuildValuesForFreeText<Category>(search);
            //}
            Expression<Func<Category, bool>> prediction = (filters != null) ? Common.CreateFilterExpression<Category>(filters) : null;
           
            var result = _categoryService.GetCategories(offset, limit, prediction, sorters);
            int total = _categoryService.Count(prediction);
            return Json(new { total = total, totalNotFiltered = total, rows = result });
        }        
    }




    public class Filter
    {
        public List<LogicBlock> Filters { get; set; }
    }

    public class LogicBlock
    {
        public List<FilterBlock> FilterBlock { get; set; }
        public string Logic { get; set; }
    }
    public class FilterBlock
    {
        public string Opand { get; set; }
        public string Column { get; set; }
        public string ColumnType { get; set; }
        public string Value { get; set; }

    }
}
