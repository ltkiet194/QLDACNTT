using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Store()
        {
            ViewBag.TrangChu = 1;
            ViewBag.Store = 0;
            return View();
        }
    }
}