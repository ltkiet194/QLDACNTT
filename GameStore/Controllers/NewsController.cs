﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult News()
        {
            ViewBag.TrangChu = 1;
            return View();
        }
    }
}