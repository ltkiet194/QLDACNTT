using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class TestController : Controller
    {
        // GET: Test

        //QUy Thay JK
        public ActionResult Index()
        {
            int a = 5;

            return View();
        }
    }
}