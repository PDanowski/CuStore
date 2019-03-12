using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace CuStore.WebUI.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            ViewBag.Message = "CuStore - customizable store";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "contanct@custore.com";

            return View();
        }
    }
}