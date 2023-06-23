using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;

namespace GameStore.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: TrangChu
        GameStoreDataContext db = new GameStoreDataContext();
        public ActionResult TrangChu()
        {
            ViewBag.TrangChu = 0;
            return View();
        }
        public KHACHHANG LayKhachHang()
        {
            KHACHHANG sv = Session["KhachHang"] as KHACHHANG;
            return sv;
        }

        public ActionResult PartialTimKiemVaLienKet()
        {
            return PartialView();
        }
        public ActionResult PartialTopVideo()
        {
            return PartialView();
        }
        public ActionResult PartialTop3Game()
        {
            return PartialView();
        }
        public ActionResult PartialPopularGame()
        {
            return PartialView();
        }
        public ActionResult PartialNav()
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            var listbb = db.BANBEs.Where(n => n.Id_KH == kh.MaKH).ToList();
            
            return PartialView();
        }

        public ActionResult PartialSlider()
        {
            return PartialView();
        }
        public ActionResult PartialLatestNews()
        {
            return PartialView();
        }
        public ActionResult PartialFooter()
        {
            return PartialView();
        }
        public ActionResult PartialSearch()
        {
            return PartialView();
        }
        public ActionResult PartialPcPsXbox()
        {
            return PartialView();
        }
    }
}