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

        public IActionResult LoadData()
        {
            var draw = int.Parse(HttpContext.Request.Form["draw"].FirstOrDefault());
            // Skiping number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();
            // Paging Length 10,20  
            var length = Request.Form["length"].FirstOrDefault();
            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction ( asc ,desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            //Paging Size (10,20,50,100)  
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = _categoryService.Count();

            var data = _categoryService.GetCategories(skip, pageSize);

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        
    }
}
