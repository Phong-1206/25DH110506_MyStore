using System;
using System.Linq;
using System.Web.Mvc;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Models.ViewModel;
using _25DH110506_MyStore.Services;

namespace _25DH110506_MyStore.Controllers
{
    [Authorize] // Bắt buộc đăng nhập mới được thanh toán
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        private ICartService GetCartService()
        {
            return new SessionCartService(db, HttpContext);
        }

        // ============================================================
        // GET: /Order/Checkout  -> Hiển thị form nhập địa chỉ
        // ============================================================
        public ActionResult Checkout()
        {
            var cart = GetCartService().GetCart();
            if (cart.IsEmpty)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutVM
            {
                Cart = cart,
                // Lấy sẵn địa chỉ của User từ DB để điền vào form (nếu có)
                AddressDelivery = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name)?.Address
            };

            return View(model);
        }

        // ============================================================
        // POST: /Order/PlaceOrder  -> Lưu Đơn hàng vào Database
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
                model.Cart = cart; // Gắn lại giỏ hàng để hiển thị nếu có lỗi
                return View("Checkout", model);
            }

            try
            {
                // 1. Tạo Hóa Đơn (Order)
                var order = new Order
                {
                    Username = User.Identity.Name, // Lấy Username từ Cookie đăng nhập
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.TotalPrice,
                    PaymentStatus = "Chờ xử lý", // Mặc định
                    AddressDelivery = model.AddressDelivery
                };

                db.Orders.Add(order);
                db.SaveChanges(); // Lưu trước để lấy OrderID tự tăng

                // 2. Tạo Chi Tiết Hóa Đơn (OrderDetail)
                foreach (var item in cart.Items)
                {
                    var detail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.ProductPrice // Lưu lại giá tại thời điểm mua
                    };
                    db.OrderDetails.Add(detail);
                }

                db.SaveChanges();

                // 3. Xóa giỏ hàng sau khi đặt thành công
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
        // GET: /Order/OrderSuccess/5 -> Trang thông báo thành công
        // ============================================================
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.OrderID == id && o.Username == User.Identity.Name);
            if (order == null) return HttpNotFound();
            return View(order);
        }
    }
}