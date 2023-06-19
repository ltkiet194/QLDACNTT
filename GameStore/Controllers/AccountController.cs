using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GameStore.Models;

namespace GameStore.Controllers
{
    public class AccountController : Controller
    {
        DataGameStoreDataContext db = new DataGameStoreDataContext();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult CheckLogin(string strs)
        {
            try
            {
                string[] list = strs.Split('|');
                int check = db.KHACHHANGs.Where(s => s.TaiKhoan == list[0]).Count();
                var getkh = db.KHACHHANGs.Where(s => s.TaiKhoan == list[0]).SingleOrDefault();
                if(check == 1)
                {
                    Session["KhachHang"] = getkh;
                }    
                if (check <1)
                {
                    KHACHHANG kh = new KHACHHANG();                  
                    kh.TaiKhoan = list[0];
                    kh.HoTen = list[1];
                    db.KHACHHANGs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    Session["KhachHang"] = kh;
                }
                return Json(new { code = 200, dt = list, msg = "Lay thong tin thanh cong." },
                         JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lay thong tin that bai " + ex.Message },
                   JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["KhachHang"] = null;
            return RedirectToAction("TrangChu", "TrangChu");
        }
    }
    
}