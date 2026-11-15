using System.Linq;
using System.Web.Mvc;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Models.ViewModel;
using _25DH110506_MyStore.Helpers;

namespace _25DH110506_MyStore.Areas.Admin.Controllers
{
    [AdminOnly] // Bảo vệ trang Admin
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var model = new DashboardVM();

            // 1. Đếm tổng số đơn hàng
            model.TotalOrders = db.Orders.Count();

            // 2. Đếm tổng số khách hàng
            model.TotalCustomers = db.Customers.Count();

            // 3. Tính tổng doanh thu (Chỉ tính các đơn không bị hủy)
            model.TotalRevenue = db.Orders
                .Where(o => o.PaymentStatus != "Đã hủy")
                .Select(o => o.TotalAmount)
                .DefaultIfEmpty(0)
                .Sum();

            // 4. Lấy dữ liệu cho biểu đồ tròn (Group by Trạng thái)
            var statusData = db.Orders
                .GroupBy(o => o.PaymentStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            // Xử lý trường hợp Database chưa có trạng thái nào (PaymentStatus bị null)
            model.OrderStatusLabels = statusData
                .Select(s => string.IsNullOrEmpty(s.Status) ? "Không xác định" : s.Status)
                .ToList();
            model.OrderStatusData = statusData.Select(s => s.Count).ToList();

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}