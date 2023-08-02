using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.Models;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class AdCommunityController : Controller
    {
        GameStoreDataContext db = new GameStoreDataContext();
        // GET: Admin/AdCommunity
        public ActionResult VerifiedPost()
        {
            return View(db.BAIDANGCONGDONGs.Where(m=>m.KiemDuyet == false).OrderByDescending(m=>m.NgayDang).ToList());
        }

        [HttpPost]
        public ActionResult Verified(int id)
        {
            try {
                var getPost = db.BAIDANGCONGDONGs.SingleOrDefault(n => n.Id_BaiDang == id);
                getPost.KiemDuyet = true;
                db.SubmitChanges();
                return Json(new { success = true, message = "Verified Post Success", JsonRequestBehavior.AllowGet }); 
            }
           
            catch(Exception ex)
            {
                return Json(new { success = false, message = "fali" + ex.Message, JsonRequestBehavior.AllowGet }); 
            }
           
        }
        [HttpPost]
        public ActionResult DeltetePost (int id)
        {
            try
            {
                var getPost = db.BAIDANGCONGDONGs.SingleOrDefault(n => n.Id_BaiDang == id);
                db.BAIDANGCONGDONGs.DeleteOnSubmit(getPost);
                db.SubmitChanges();
                return Json(new { success = true, message = "Delete Post Success", JsonRequestBehavior.AllowGet });
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = "fali" + ex.Message, JsonRequestBehavior.AllowGet });
            }

        }
    }
}