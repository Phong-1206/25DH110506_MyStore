using _25DH110506_MyStore.Helpers;
using _25DH110506_MyStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using _25DH110506_MyStore.Helpers;

namespace _25DH110506_MyStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            SeedAdmin();
        }
        private void SeedAdmin()
        {
            try
            {
                using (var db = new MyStoreEntities())
                {
                    // Nếu đã có ít nhất 1 admin → bỏ qua
                    if (db.Users.Any(u => u.UserRole == "A"))
                        return;

                    var admin = new User
                    {
                        Username = "admin",
                        Password = PasswordHelper.Hash("admin123"),
                        UserRole = "A",
                        FullName = "Quản trị viên",
                        Email = "admin@ttp.com",
                        Phone = "0123456789",
                        Address = "Hồ Chí Minh",
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    db.Users.Add(admin);
                    db.SaveChanges();

                    System.Diagnostics.Debug.WriteLine("=== Đã tạo admin: admin / admin123");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== Lỗi seed admin: {ex.Message}");
            }
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(
        $"=== [Auth] Path={Context.Request.Path}, Cookie={(Context.Request.Cookies[FormsAuthentication.FormsCookieName] != null ? "YES" : "NO")}");
            // ✅ Log để verify đang chạy
            System.Diagnostics.Debug.WriteLine(
                $"=== [Auth] Path={Context.Request.Path}, HasCookie={Context.Request.Cookies[FormsAuthentication.FormsCookieName] != null}");

            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value)) return;

            FormsAuthenticationTicket ticket;
            try { ticket = FormsAuthentication.Decrypt(authCookie.Value); }
            catch { return; }
            if (ticket == null || ticket.Expired) return;

            string role = ticket.UserData;
            if (string.IsNullOrEmpty(role)) return;

            System.Diagnostics.Debug.WriteLine(
                $"=== [Auth] User={ticket.Name}, Role={role}");   // ← xác nhận role đọc đúng

            var identity = new GenericIdentity(ticket.Name, "Forms");
            Context.User = new GenericPrincipal(identity, new[] { role });
        }
    
    }
}
