using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class BlogListController : Controller
    {
        // GET: BlogArticle
        public ActionResult BlogList()
        {
            ViewBag.TrangChu =1;
            return View();
        }
    }
}