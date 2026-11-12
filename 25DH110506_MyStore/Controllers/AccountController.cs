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

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
                return View(model);
            }

            SignInUser(user, model.RememberMe);
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
            if (!ModelState.IsValid)
            {
                TempData["RegisterErrors"] = string.Join("<br/>",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["ActiveTab"] = "register";
                return RedirectToAction("Login");
            }

            if (db.Users.Any(u => u.Username == model.UserName))
            {
                TempData["RegisterErrors"] = "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.";
                TempData["ActiveTab"] = "register";
                return RedirectToAction("Login");
            }

            var user = new User
            {
                Username = model.UserName,
                Password = PasswordHelper.Hash(model.Password),
                UserRole = "C",   // Customer mặc định
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            db.Users.Add(user);
            db.SaveChanges();

            SignInUser(user, false);
            TempData["Success"] = "Đăng ký thành công! Chào mừng " + user.FullName;
            return RedirectToAction("Index", "Home");
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
            var user = GetCurrentUser();
            if (user == null) return HttpNotFound();

            var model = new ProfileInfoVM
            {
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address
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
            if (!ModelState.IsValid)
                return View("ProfileInfo", model);

            var user = GetCurrentUser();
            if (user == null) return HttpNotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Address = model.Address;

            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
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
            var user = GetCurrentUser();
            if (user == null) return new ProfileInfoVM();
            return new ProfileInfoVM
            {
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address
            };
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult _CheckRole()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Username:           {User?.Identity?.Name ?? "(null)"}<br/>");
            sb.AppendLine($"IsAuthenticated:    {User?.Identity?.IsAuthenticated}<br/>");
            sb.AppendLine($"AuthenticationType: {User?.Identity?.AuthenticationType ?? "(null)"}<br/>");
            sb.AppendLine($"IsInRole('A'):      {User.IsInRole("A")}<br/>");
            sb.AppendLine($"IsInRole('C'):      {User.IsInRole("C")}<br/>");
            sb.AppendLine($"User.GetType():     {User?.GetType().FullName ?? "(null)"}<br/>");
            sb.AppendLine($"Identity.GetType(): {User?.Identity?.GetType().FullName ?? "(null)"}<br/>");

            return Content(sb.ToString(), "text/html");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}