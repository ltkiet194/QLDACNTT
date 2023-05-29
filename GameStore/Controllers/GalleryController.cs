using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Gallery()
        {
            ViewBag.TrangChu = 1;
            return View();
        }
    }
}