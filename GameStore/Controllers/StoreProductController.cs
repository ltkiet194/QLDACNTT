using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class StoreProductController : Controller
    {
        // GET: StoreProduct
        public ActionResult StoreProduct()
        {
            ViewBag.TrangChu = 1;
            ViewBag.StoreProduct = 1;
            return View();
        }
        public ActionResult PartialCategory()
        {
            return PartialView();
        }
        public ActionResult PartialPriceFilter()
        {
            return PartialView();
        }
        public ActionResult PartialGameEnviroment()
        {
            // PC or PS or Xbox..
            return PartialView();
        }
    }
}