using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        // GET: Admin/LoginAdmin
        GameStoreDataContext db = new GameStoreDataContext();

        // GET: Admin/LoginAdmin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string TenDN, string MatKhau)
        {
            // Kiểm tra thông tin đăng nhập
            var admin = db.ADMINs.FirstOrDefault(a => a.TenDN == TenDN && a.MatKhau == MatKhau);

            if (admin != null)
            {
                // Đăng nhập thành công, tiến hành lưu thông tin vào session
                Session["Admin"] = admin;

                // Chuyển hướng đến trang quản lý sau khi đăng nhập thành công
                return RedirectToAction("Index", "TrangChuAdmin");
            }

            // Đăng nhập thất bại, hiển thị thông báo lỗi
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác.";
            return View();
        }
        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("Index", "TrangChuAdmin");
        }
    }
}