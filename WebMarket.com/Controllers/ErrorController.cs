using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMarket.com.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            ViewBag.PageTitle = "صفحه مورد نظر پیدا نشد";
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}