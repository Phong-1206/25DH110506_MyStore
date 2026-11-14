using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Helpers; // Nếu dùng AdminOnlyAttribute

namespace _25DH110506_MyStore.Areas.Admin.Controllers
{
    [AdminOnly] // Chỉ Admin mới được vào
    public class OrdersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Orders
        public ActionResult Index(string searchTerm)
        {
            // Lấy danh sách đơn hàng kèm thông tin Khách hàng
            var orders = db.Orders.Include(o => o.Customer).AsQueryable();

            // Tìm kiếm theo Tên khách hàng hoặc Mã đơn
            if (!string.IsNullOrEmpty(searchTerm))
            {
                orders = orders.Where(o => o.Customer.CustomerName.Contains(searchTerm)
                                        || o.OrderID.ToString().Contains(searchTerm));
            }

            // Sắp xếp đơn mới nhất lên đầu
            return View(orders.OrderByDescending(o => o.OrderDate).ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Lấy thông tin đơn hàng
            var order = db.Orders.Include(o => o.Customer).FirstOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();

            // Lấy chi tiết các sản phẩm trong đơn
            var details = db.OrderDetails.Include(d => d.Product).Where(d => d.OrderID == id).ToList();
            ViewBag.OrderDetails = details;

            return View(order);
        }

        // POST: Admin/Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int orderId, string status)
        {
            try
            {
                var order = db.Orders.Find(orderId);
                if (order == null)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng!";
                    return RedirectToAction("Index");
                }

                order.PaymentStatus = status;
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = $"Đã cập nhật trạng thái đơn hàng #{orderId} thành: {status}";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }

            // Load lại trang Details
            return RedirectToAction("Details", new { id = orderId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}