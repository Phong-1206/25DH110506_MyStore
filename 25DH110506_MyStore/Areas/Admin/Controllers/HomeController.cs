using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25DH110506_MyStore.Helpers;
using static _25DH110506_MyStore.Helpers.AdminOnlyAttribute;

namespace _25DH110506_MyStore.Areas.Admin.Controllers
{
    [AdminOnly]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
    }
}