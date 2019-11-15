using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMarket.com.Controllers
{
    public class ManageEmailController : Controller
    {
        // GET: ManageEmail
        public ActionResult ActivateEmail()
        {
            return PartialView();
        }
        public ActionResult RecoverPassword()
        {
            return PartialView();
        }
        public ActionResult SendAdvertising()
        {
            return PartialView();
        }
    }
}