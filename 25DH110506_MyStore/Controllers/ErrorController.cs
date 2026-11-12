using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _25DH110506_MyStore.Controllers
{
    public class ErrorController : Controller
    {
        [AllowAnonymous]        // ⚠️ Bắt buộc — để role C vẫn vào được trang lỗi
        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;   // ⚠️ IIS không override bằng trang lỗi mặc định
            return View();
        }
    }
}