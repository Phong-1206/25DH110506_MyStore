using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Models.ViewModel;
using _25DH110506_MyStore.Services;

namespace _25DH110506_MyStore.Controllers
{
    [Authorize] // Bắt buộc đăng nhập
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        private ICartService GetCartService()
        {
            return new SessionCartService(db, HttpContext);
        }

        // ============================================================
        // GET: /Order/Checkout
        // ============================================================
        public ActionResult Checkout()
        {
            var cart = GetCartService().GetCart();
            if (cart.IsEmpty)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);

            var model = new CheckoutVM
            {
                Cart = cart,
                AddressDelivery = customer?.CustomerAddress
            };

            return View(model);
        }

        // ============================================================
        // POST: /Order/PlaceOrder
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(CheckoutVM model)
        {
            var cart = GetCartService().GetCart();

            if (cart.IsEmpty)
            {
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                model.Cart = cart;
                return View("Checkout", model);
            }

            try
            {
                var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);
                if (customer == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng. Vui lòng cập nhật hồ sơ.");
                    model.Cart = cart;
                    return View("Checkout", model);
                }
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.TotalPrice,
                    PaymentStatus = "Chờ xử lý",
                    AddressDelivery = model.AddressDelivery
                };

                db.Orders.Add(order);
                db.SaveChanges();
                foreach (var item in cart.Items)
                {
                    var detail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.ProductPrice
                    };
                    db.OrderDetails.Add(detail);
                }

                db.SaveChanges();
                GetCartService().ClearCart();

                TempData["Success"] = "Đặt hàng thành công! Mã đơn hàng của bạn là #" + order.OrderID;
                return RedirectToAction("OrderSuccess", new { id = order.OrderID });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi đặt hàng: " + ex.Message);
                model.Cart = cart;
                return View("Checkout", model);
            }
        }

        // ============================================================
        // GET: /Order/OrderSuccess/5
        // ============================================================
        public ActionResult OrderSuccess(int id)
        {
            // Kiểm tra xem Order này có thuộc về Customer đang đăng nhập không
            var order = db.Orders.FirstOrDefault(o => o.OrderID == id && o.Customer.Username == User.Identity.Name);

            if (order == null)
                return HttpNotFound();

            return View(order);
        }

        /*public ActionResult MyOrders(int? page)
        {
            var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);
            if (customer == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToAction("Index", "Home");
            }
            var orders = db.Orders
                .Include(o => o.Customer)
                .Where(o => o.CustomerID == customer.CustomerID)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }*/

        // ============================================================
        // GET: /Order/MyOrderDetails/5  — Chi tiết một đơn
        // ============================================================
        public ActionResult MyOrderDetails(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var order = db.Orders
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.OrderID == id && o.Customer.Username == User.Identity.Name);

            if (order == null) return HttpNotFound();
            ViewBag.OrderDetails = db.OrderDetails
                .Include(d => d.Product)
                .Where(d => d.OrderID == id)
                .ToList();

            return View(order);
        }
        public ActionResult MyOrders(string searchTerm)
        {
            var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);
            if (customer == null) return RedirectToAction("Index", "Home");

            var orders = db.Orders.Include(o => o.Customer).Where(o => o.CustomerID == customer.CustomerID);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                int orderId;
                if (int.TryParse(searchTerm, out orderId))
                {
                    orders = orders.Where(o => o.OrderID == orderId);
                }
            }

            var result = orders.OrderByDescending(o => o.OrderDate).ToList();
            ViewBag.SearchTerm = searchTerm; 

            return View(result);
        }

        // ============================================================
        // POST: /Order/BuyAgain  — Mua lại toàn bộ đơn hàng
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyAgain(int orderId)
        {
            var order = db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderID == orderId && o.Customer.Username == User.Identity.Name);

            if (order == null) return HttpNotFound();

            var cartService = GetCartService();
            int addedCount = 0;
            foreach (var detail in order.OrderDetails)
            {
                var product = db.Products.Find(detail.ProductID);
                if (product != null)
                {
                    cartService.AddItem(detail.ProductID, 1); //  = 1
                    addedCount++;
                }
            }

            if (addedCount > 0)
            {
                TempData["Success"] = $"Đã thêm {addedCount} sản phẩm từ đơn hàng #{orderId} vào giỏ hàng!";
            }
            else
            {
                TempData["Error"] = "Rất tiếc, các sản phẩm trong đơn hàng này đã ngừng kinh doanh.";
            }
            return RedirectToAction("Index", "Cart");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}