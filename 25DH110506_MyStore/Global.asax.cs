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
                    if (!db.Users.Any(u => u.UserRole == "A"))
                    {
                        // 1. Tạo User
                        var adminUser = new User
                        {
                            Username = "admin",
                            Password = PasswordHelper.Hash("admin123"),
                            UserRole = "A"
                        };
                        db.Users.Add(adminUser);
                        db.SaveChanges();

                        // 2. Tạo Customer profile cho admin
                        var adminCustomer = new Customer
                        {
                            Username = "admin",
                            CustomerName = "Quản trị viên",
                            CustomerPhone = "0123456789",
                            CustomerEmail = "admin@mystore.com",
                            CustomerAddress = "Hồ Chí Minh"
                        };
                        db.Customers.Add(adminCustomer);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== [Seed] ❌ LỖI: {ex.Message}");
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
