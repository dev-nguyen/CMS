using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
