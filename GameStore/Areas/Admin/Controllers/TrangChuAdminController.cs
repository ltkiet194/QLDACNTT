﻿using GameStore.Models;
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
            DateTime today = DateTime.Today;
            DateTime yesterday = DateTime.Today.AddDays(-1);
            ViewBag.SoluongOL = db.KHACHHANGs.Where(m=>m.LastActivity == true).Count();
            
            ViewBag.TongDoanhThu = db.DONHANGs.Where(m =>m.NgayMua.Value.Date == today).Sum(m=>m.TongTien);
            ViewBag.TongDoanhThuHomQua = db.DONHANGs.Where(m => m.NgayMua != null && m.NgayMua.Value.Date == yesterday).Sum(m => m.TongTien);
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
        public JsonResult GetOnlineUserCount()
        {
            int onlineUserCount = db.KHACHHANGs.Where(m => m.LastActivity == true).Count(); 

            return Json(new { onlineUserCount }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSLBuyGame()
        {
            // Lấy ngày hôm nay
            DateTime today = DateTime.Today;

            // Lấy số lượng đơn hàng mua trong ngày hôm nay
            int numberOfOrders = db.DONHANGs.Count(m => m.NgayMua.Value.Date == today);
            return Json(new { numberOfOrders }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTotalToday()
        {
            // Lấy ngày hôm nay
            DateTime today = DateTime.Today;

            // Lấy số lượng đơn hàng mua trong ngày hôm nay
            var numberOfSumTotal = db.DONHANGs.Where(m =>m.NgayMua.Value.Date == today).Sum(m => m.TongTien);
            return Json(new { numberOfSumTotal }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTotalYesterday()
        {
            // Lấy ngày hôm qua
            DateTime yesterday = DateTime.Today.AddDays(-1);

            // Lấy số lượng đơn hàng mua trong ngày hôm qua
            var totalAmountYesterday = db.DONHANGs.Where(m => m.NgayMua != null && m.NgayMua.Value.Date == yesterday).Sum(m => m.TongTien);

            return Json(new { totalAmountYesterday }, JsonRequestBehavior.AllowGet);
        }
    }
}