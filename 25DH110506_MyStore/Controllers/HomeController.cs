using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
namespace _25DH110506_MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        public ActionResult Index(string SearchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(SearchTerm) ||
                    p.ProductDescription.Contains(SearchTerm) ||
                    p.Category.CategoryName.Contains(SearchTerm));
            }
            int pageNumber = page ?? 1;
            int pageSize = 6;
            model.FeatureProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();
            model.NewProducts = products.OrderBy(p => p.OrderDetails.Count()).Take(20).ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}