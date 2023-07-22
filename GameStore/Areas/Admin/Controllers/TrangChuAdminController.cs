using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class TrangChuAdminController : Controller
    {
        // GET: Admin/TrangChuAdmin
        GameStoreDataContext db = new GameStoreDataContext();

        // GET: Admin/TrangChu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NavPartial()
        {
            return PartialView();
        }

        public ActionResult NavBarPartial()
        {
            return PartialView();
        }

        public ActionResult FootPartial()
        {
            return PartialView();
        }

        public ActionResult NavigationPartial()
        {
            return PartialView();
        }
    }
}