using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Helpers;

namespace _25DH110506_MyStore.Areas.Admin.Controllers
{
    [AdminOnly]
    public class CustomersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Customers
        public ActionResult Index(string searchTerm)
        {
            var customers = db.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                customers = customers.Where(c => c.CustomerName.Contains(searchTerm)
                                              || c.CustomerPhone.Contains(searchTerm));
            }

            return View(customers.ToList());
        }

        // GET: Admin/Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customer = db.Customers.Find(id);
            if (customer == null) return HttpNotFound();

            // Lấy lịch sử mua hàng của khách
            var orders = db.Orders.Where(o => o.CustomerID == id).OrderByDescending(o => o.OrderDate).ToList();
            ViewBag.Orders = orders;

            return View(customer);
        }

        // GET: Admin/Customers/Edit/5 (Admin có thể sửa thông tin KH)
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var customer = db.Customers.Find(id);
            if (customer == null) return HttpNotFound();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerName,CustomerPhone,CustomerEmail,CustomerAddress,Username")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Cập nhật thông tin khách hàng thành công!";
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}