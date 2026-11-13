using System;
using System.Web.Mvc;
using System.Linq;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Services;

namespace _25DH110506_MyStore.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        private ICartService _cartService;

        /// <summary>
        /// Lazy khởi tạo cart service — dùng chung cho mọi action
        /// </summary>
        protected ICartService GetCartService()
        {
            if (_cartService == null)
                _cartService = new SessionCartService(db, HttpContext);
            return _cartService;
        }

        // ============================================================
        // GET: /Cart
        // ============================================================
        public ActionResult Index()
        {
            var cart = GetCartService().GetCart();
            return View(cart);
        }

        // ============================================================
        // POST: /Cart/AddItem
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(int productId, int quantity = 1)
        {
            if (productId <= 0)
                return HttpNotFound();

            try
            {
                GetCartService().AddItem(productId, quantity);

                var product = db.Products.Find(productId);
                if (product != null)
                    TempData["Success"] = $"Đã thêm \"{product.ProductName}\" vào giỏ hàng!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Không thể thêm sản phẩm vào giỏ hàng: " + ex.Message;
            }

            // Quay lại trang trước đó (nếu có), mặc định về Cart
            string returnUrl = Request.UrlReferrer?.ToString();
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }

        // ============================================================
        // POST: /Cart/RemoveItem
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveItem(int productId)
        {
            if (productId <= 0)
                return HttpNotFound();

            GetCartService().RemoveItem(productId);
            TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            return RedirectToAction("Index");
        }

        // ============================================================
        // POST: /Cart/ClearCart
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            TempData["Success"] = "Đã xóa toàn bộ giỏ hàng.";
            return RedirectToAction("Index");
        }

        // ============================================================
        // POST: /Cart/UpdateQuantity
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            if (productId <= 0)
                return HttpNotFound();

            var service = GetCartService();
            service.UpdateQuantity(productId, quantity);

            // ✅ Nếu request là AJAX → trả JSON thay vì redirect
            if (Request.IsAjaxRequest())
            {
                var cart = service.GetCart();
                var item = cart.Items.FirstOrDefault(i => i.ProductID == productId);

                return Json(new
                {
                    success = true,
                    productId = productId,
                    quantity = item?.Quantity ?? 0,
                    subtotal = item?.SubTotal ?? 0m,
                    totalItems = cart.TotalItems,
                    totalPrice = cart.TotalPrice
                });
            }

            // Fallback: submit thường (nếu user tắt JS) → redirect như cũ
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}