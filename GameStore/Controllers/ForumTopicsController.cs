using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class ForumTopicsController : Controller
    {
        // GET: ForumTopics
        public ActionResult ForumTopics()
        {
            ViewBag.TrangChu = 1;
            return View();
        }
    }
}