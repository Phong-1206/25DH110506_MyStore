using System;
using System.Linq;
using System.Web.Mvc;
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

            // Tìm thông tin Customer dựa vào Username đang đăng nhập
            var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);

            var model = new CheckoutVM
            {
                Cart = cart,
                // Tự động điền địa chỉ nếu Customer đã có địa chỉ trong DB
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
                // 1. Tìm Customer tương ứng
                var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);
                if (customer == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng. Vui lòng cập nhật hồ sơ.");
                    model.Cart = cart;
                    return View("Checkout", model);
                }

                // 2. Tạo Hóa Đơn (Order) - Sử dụng CustomerID
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.TotalPrice,
                    PaymentStatus = "Chờ xử lý",
                    AddressDelivery = model.AddressDelivery
                };

                db.Orders.Add(order);
                db.SaveChanges(); // Lưu để lấy OrderID tự tăng

                // 3. Tạo Chi Tiết Hóa Đơn (OrderDetail)
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

                // 4. Xóa giỏ hàng sau khi đặt thành công
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

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}