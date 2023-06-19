using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class StoreController : Controller
    {
        DataGameStoreDataContext data = new DataGameStoreDataContext();
        // GET: Store
        public ActionResult Store()
        {
            ViewBag.TrangChu = 1;
            ViewBag.Store = 0;
            int iSize = 6;
   
            var list10Game = data.Games.ToList().Take(10);;

            return View(list10Game);
        }
        public ActionResult PartialFeaturedGame()
        {
            ViewBag.TrangChu = 1;
            ViewBag.Store = 0;
            int iSize = 6;

            var list4Game = data.Games.ToList().Take(4).Where(n => n.id <10); ;

            return PartialView(list4Game);
        }
        public ActionResult PartialMostPopular()
        {
            ViewBag.TrangChu = 1;
            ViewBag.Store = 0;
            int iSize = 6;

            var list6Game = data.Games.ToList().Take(9).Where(n => n.id < 10); ;

            return PartialView(list6Game);
        }

    }
}