using GameStore.Models;
using GameStore.Models.DAO;
using GameStore.Models.DTO.GameModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class StoreCheckoutController : Controller
    {
        // GET: StoreCheckout
        GameStoreDataContext db = new GameStoreDataContext();
        public ActionResult StoreCheckout()
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            var keyGames = db.KEYGAMEs.Where(n => n.ID_KH == kh.MaKH).OrderByDescending(n => n.DateModified).ToList();

            return View(keyGames);
        }
       
    }
}