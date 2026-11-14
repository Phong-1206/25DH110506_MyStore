using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Helpers;

namespace _25DH110506_MyStore.Areas.Admin.Controllers
{
    [AdminOnly]
    public class UsersController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Users
        public ActionResult Index()
        {
            // Chỉ lấy Username và UserRole (không lấy Password ra View)
            var users = db.Users.ToList();
            return View(users);
        }

        // POST: Admin/Users/ChangeRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRole(string username, string newRole)
        {
            var user = db.Users.Find(username);
            if (user != null)
            {
                // Không cho phép Admin tự hạ quyền chính mình (tránh mất quyền Admin)
                if (username == User.Identity.Name && newRole == "C")
                {
                    TempData["Error"] = "Bạn không thể tự giáng chức chính mình!";
                }
                else
                {
                    user.UserRole = newRole;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Success"] = $"Đã đổi quyền của '{username}' thành {(newRole == "A" ? "Admin" : "Khách hàng")}.";
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Admin/Users/Delete (Xóa tài khoản)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string username)
        {
            var user = db.Users.Find(username);
            if (user == null) return HttpNotFound();

            if (username == User.Identity.Name)
            {
                TempData["Error"] = "Bạn không thể tự xóa tài khoản của chính mình!";
                return RedirectToAction("Index");
            }

            try
            {
                // Lưu ý: Nếu User này đã có Customer/Order, bạn phải xóa Customer/Order trước hoặc xử lý Soft Delete
                var customer = db.Customers.FirstOrDefault(c => c.Username == username);
                if (customer != null) db.Customers.Remove(customer);

                db.Users.Remove(user);
                db.SaveChanges();
                TempData["Success"] = "Đã xóa tài khoản thành công.";
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Không thể xóa: Tài khoản này đang có dữ liệu đơn hàng. " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}