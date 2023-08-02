using GameStore.Models;
using GameStore.Models.DTO.GameModel;
using Newtonsoft.Json;
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
        GameStoreDataContext db = new GameStoreDataContext();
        public ActionResult Gallery()
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];

            var GetKH = db.KHACHHANGs.SingleOrDefault(k => k.MaKH == kh.MaKH);
            List<GamesInCart> games = JsonConvert.DeserializeObject<List<GamesInCart>>(GetKH.ListGameInLibrary);
            return View(games);
        }
    }
}