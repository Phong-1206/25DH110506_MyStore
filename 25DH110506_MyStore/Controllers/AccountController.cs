using _25DH110506_MyStore.Helpers;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _25DH110506_MyStore.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // ============================================================
        // GET: /Account/Login
        // ============================================================
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // ============================================================
        // POST: /Account/Login
        // ============================================================
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var user = db.Users.FirstOrDefault(u => u.Username == model.UserName);

            if (user == null || !PasswordHelper.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(model);
            }

            SignInUser(user, model.RememberMe);

            // ⚠️ SỬA DÒNG NÀY (Lấy tên từ bảng Customer thay vì User)
            var customer = db.Customers.FirstOrDefault(c => c.Username == user.Username);
            string displayName = customer != null ? customer.CustomerName : user.Username;
            TempData["Success"] = "Xin chào, " + displayName;

            return RedirectToLocal(returnUrl);
        }

        // ============================================================
        // POST: /Account/Register
        // ============================================================
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Tạo tài khoản đăng nhập (User)
                var user = new User
                {
                    Username = model.UserName,
                    Password = PasswordHelper.Hash(model.Password),
                    UserRole = "C" // Khách hàng
                };
                db.Users.Add(user);
                db.SaveChanges();

                // 2. Tạo thông tin khách hàng (Customer) liên kết với Username
                var customer = new Customer
                {
                    Username = model.UserName,
                    CustomerName = model.FullName,
                    CustomerPhone = model.Phone ?? "Chưa cập nhật",
                    CustomerEmail = model.Email,
                    CustomerAddress = model.Address
                };
                db.Customers.Add(customer);
                db.SaveChanges();

                // 3. Đăng nhập luôn
                SignInUser(user, false);
                TempData["Success"] = "Đăng ký thành công!";
                return RedirectToAction("Index", "Home");
            }

            // Xử lý lỗi...
            TempData["ActiveTab"] = "register";
            return RedirectToAction("Login");
        }

        // ============================================================
        // POST: /Account/Logout
        // ============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // ============================================================
        // GET: /Account/ProfileInfo
        // ============================================================
        [Authorize]
        public ActionResult ProfileInfo()
        {
            // Tìm thông tin User đang đăng nhập
            var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return HttpNotFound();

            // Tìm thông tin Customer tương ứng
            var customer = db.Customers.FirstOrDefault(c => c.Username == user.Username);

            var model = new ProfileInfoVM
            {
                Username = user.Username,
                FullName = customer?.CustomerName,
                Email = customer?.CustomerEmail,
                Phone = customer?.CustomerPhone,
                Address = customer?.CustomerAddress
            };
            return View(model);
        }

        // ============================================================
        // POST: /Account/UpdateProfile
        // ============================================================
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(ProfileInfoVM model)
        {
            if (!ModelState.IsValid) return View("ProfileInfo", model);

            var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);
            if (customer == null) return HttpNotFound();

            // Cập nhật thông tin
            customer.CustomerName = model.FullName;
            customer.CustomerEmail = model.Email;
            customer.CustomerPhone = model.Phone;
            customer.CustomerAddress = model.Address;

            db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("ProfileInfo");
        }

        // ============================================================
        // POST: /Account/ChangePassword
        // ============================================================
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
                return View("ProfileInfo", ReloadProfileVM());

            var user = GetCurrentUser();
            if (user == null) return HttpNotFound();

            if (!PasswordHelper.Verify(model.OldPassword, user.Password))
            {
                ModelState.AddModelError("", "Mật khẩu hiện tại không đúng.");
                return View("ProfileInfo", ReloadProfileVM());
            }

            user.Password = PasswordHelper.Hash(model.NewPassword);
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            // Đổi pass KHÔNG logout vì FormsAuth không track password hash
            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("ProfileInfo");
        }


        // ============================================================
        // HELPERS
        // ============================================================

        /// <summary>
        /// Đăng nhập: tạo FormsAuthenticationTicket có UserData = UserRole.
        /// Global.asax sẽ đọc ticket này để set User.IsInRole().
        /// </summary>
        private void SignInUser(User user, bool rememberMe)
        {
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: user.Username,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddDays(rememberMe ? 30 : 1),
                isPersistent: rememberMe,
                userData: user.UserRole     // ← "A" hoặc "C"
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true
            };
            if (rememberMe) cookie.Expires = ticket.Expiration;

            Response.Cookies.Add(cookie);
        }

        private User GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated) return null;
            string username = User.Identity.Name;
            return db.Users.FirstOrDefault(u => u.Username == username);
        }

        private ProfileInfoVM ReloadProfileVM()
        {
            var customer = db.Customers.FirstOrDefault(c => c.Username == User.Identity.Name);
            if (customer == null) return new ProfileInfoVM();

            return new ProfileInfoVM
            {
                Username = customer.Username,
                FullName = customer.CustomerName,
                Email = customer.CustomerEmail,
                Phone = customer.CustomerPhone,
                Address = customer.CustomerAddress
            };
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}