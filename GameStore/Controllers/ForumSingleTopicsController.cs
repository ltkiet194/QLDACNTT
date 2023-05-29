using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class ForumSingleTopicsController : Controller
    {
        // GET: ForumSingleTopics
        public ActionResult ForumSingleTopics()
        {
            ViewBag.TrangChu = 1;
            return View();
        }
    }
}