using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Admin/Profile
        GameStoreDataContext db = new GameStoreDataContext();

        // GET: Admin/Profile
        public ActionResult Profile()
        {
            // Truy vấn dữ liệu từ bảng ADMIN
            ADMIN admin = db.ADMINs.FirstOrDefault();

            // Truyền dữ liệu vào view thông qua model
            return View(admin);
        }
    }
}