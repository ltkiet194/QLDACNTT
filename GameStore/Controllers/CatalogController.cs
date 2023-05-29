using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class CatalogController : Controller
    {
        // GET: Catalog
        public ActionResult Catalog()
        {
            ViewBag.TrangChu = 0;
            ViewBag.StoreProduct = 0;
            return View();
        }
    }
}