using GameStore.Models;
using GameStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class CommunityController : Controller
    {
        // GET: Community
        GameStoreDataContext db = new GameStoreDataContext();
        public ActionResult SocialNetwork()
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            var posts = db.BAIDANGCONGDONGs.OrderByDescending(n=>n.NgayDang).ToList();
         

            // Fetch the user information for each post and add it to the ViewBag (or use a ViewModel)
            var userIds = posts.Select(p => p.Id_NguoiDang).ToList();
            var users = db.KHACHHANGs.Where(u => userIds.Contains(u.MaKH))
                                     .ToDictionary(u => u.MaKH, u => u);

            ViewBag.Users = users;

            return View(posts);
        }

        [HttpPost]
        public ActionResult Postnew(HttpPostedFileBase Picturenew, string content)
        {
            if (ModelState.IsValid)
            {
                if (Picturenew != null && Picturenew.ContentLength > 0)
                {
                    try
                    {
                        // Kiểm tra kích thước tệp tin ảnh
                        if (!IsFileSizeValid(Picturenew))
                        {
                            return Json(new { success = false, message = "Image file size exceeds the limit. Please upload an image file no larger than 5 MB." });
                        }

                        // Tạo tên tệp tin duy nhất
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Picturenew.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Theme/img"), fileName);
                        Picturenew.SaveAs(filePath);

                        // Cập nhật đường dẫn ảnh hồ sơ trong cơ sở dữ liệu (theo mã khách hàng của bạn)
                        KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                        BAIDANGCONGDONG bd = new BAIDANGCONGDONG();
                        bd.Id_NguoiDang = kh.MaKH;
                        bd.NoiDung = content;
                        bd.TieuDe = null;
                        bd.HinhAnh= $"/Theme/img/{fileName}";// Lưu đường dẫn dựa trên src của thẻ <img>
                        bd.NgayDang = DateTime.Now;
                        bd.KiemDuyet = false;
                        bd.CountLike = 0;
                        bd.ListComment = "[]";
                        db.BAIDANGCONGDONGs.InsertOnSubmit(bd);
                        db.SubmitChanges();

                        return Json(new { success = true, message = "Post successful. Pending pending!." });
                    }
                    catch (Exception)
                    {
                        return Json(new { success = false, message = "Fail. Please try again." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "No image file selected." });
                }
            }

            return Json(new { success = false, message = "No image file selected." });
        }
        private bool IsFileSizeValid(HttpPostedFileBase file)
        {
            const int maxFileSize = 5 * 1024 * 1024; // 5 MB
            return file.ContentLength <= maxFileSize;
        }
        public ActionResult AddLike(int id)
        {
            try {
                KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                var bd = db.BAIDANGCONGDONGs.SingleOrDefault(n => n.Id_BaiDang == id);

                if (string.IsNullOrEmpty(bd.ListUserLike))
                {
                    bd.ListUserLike = "[]"; // Initialize the list if it is empty or null
                }
                var data = JsonConvert.DeserializeObject<List<UserLike>>(bd.ListUserLike);

                // Check if the user has already liked the post
                var existingLike = data.FirstOrDefault(l => l.Id_kh == kh.MaKH && l.Id_Bd == id);
                if (existingLike != null)
                {
                    // If the user has already liked the post, decrement the like count and remove the like entry
                    bd.CountLike -= 1;
                    data.Remove(existingLike);
                }
                else
                {
                    // If the user has not liked the post, increment the like count and add the like entry
                    bd.CountLike += 1;
                    data.Add(new UserLike { Id_kh = kh.MaKH, Id_Bd = id, NgayLike = DateTime.Now });
                }

                // Serialize the updated data back to JSON
                bd.ListUserLike = JsonConvert.SerializeObject(data);
                db.SubmitChanges();
                return Json(new { success = true, message = "OK",data = bd.CountLike });
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Fail " + ex.Message },
                  JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteNew(int id)
        {
            try
            {             
                var bd = db.BAIDANGCONGDONGs.SingleOrDefault(n => n.Id_BaiDang == id);
                db.BAIDANGCONGDONGs.DeleteOnSubmit(bd);
                db.SubmitChanges();
                return Json(new { success = true, message = "Post deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Fail " + ex.Message },
                  JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public ActionResult CommentParent(FormCollection f)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];            
            if (ModelState.IsValid)
            {
                if (f["textCommentContent"] == "")
                {
                    return View("SocialNetwork");
                }
                var baidang = db.BAIDANGCONGDONGs.SingleOrDefault(n=>n.Id_BaiDang==int.Parse(f["id"]));
                List<CommentNew> list = JsonConvert.DeserializeObject<List<CommentNew>>(baidang.ListComment);
                CommentNew cmt = new CommentNew
                {
                    Id_Post = int.Parse(f["id"]),
                    Id_Conmment = list.Count(),
                    Id_kh = kh.MaKH,
                    Avatar = kh.Avatar,
                    IdParent = -1,
                    Content = f["textCommentContent"],
                    Name = kh.HoTen,
                    DateModified = DateTime.Now
                };
                list.Add(cmt);
                baidang.ListComment = JsonConvert.SerializeObject(list);
                db.SubmitChanges();

            }

            return RedirectToAction("SocialNetwork","Community");
        }


        [HttpPost]
        public ActionResult CommentChild(FormCollection f)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            if (ModelState.IsValid)
            {
                if (f["textCommentContent"] == "")
                {
                    return View("SocialNetwork");
                }
                string[] itemId = f["id"].Split('-');
                int idBd = int.Parse(itemId[0]);
                int idCmt = int.Parse(itemId[1]);

                var baidang = db.BAIDANGCONGDONGs.SingleOrDefault(n => n.Id_BaiDang == idBd);
                List<CommentNew> list = JsonConvert.DeserializeObject<List<CommentNew>>(baidang.ListComment);
                CommentNew cmt = new CommentNew
                {
                    Id_Post = idBd,
                    Id_Conmment = list.Count(),
                    Id_kh = kh.MaKH,
                    Avatar = kh.Avatar,
                    IdParent = idCmt,
                    Content = f["textCommentContent"],
                    Name = kh.HoTen,
                    DateModified = DateTime.Now
                };
                list.Add(cmt);
                baidang.ListComment = JsonConvert.SerializeObject(list);
                db.SubmitChanges();

            }

            return RedirectToAction("SocialNetwork", "Community");
        }




        [ChildActionOnly]
        public ActionResult LoadChildComment(int idBaiDang,int parentId)
        {
            List<CommentNew> lst = new List<CommentNew>();

            var liscm = db.BAIDANGCONGDONGs.Where(n => n.Id_BaiDang == idBaiDang).SingleOrDefault();
            List<CommentNew> cm = JsonConvert.DeserializeObject<List<CommentNew>>(liscm.ListComment);
         
            ViewBag.Count = lst.Count();
            lst = cm.Where(m => m.IdParent == parentId).ToList();
            int[] a = new int[cm.Count()];
            for (int ia = 0; ia < lst.Count; ia++)
            {
                var l = cm.Where(m => m.IdParent == lst[ia].Id_Conmment);
                a[ia] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView("LoadChildComment", lst);
        }


    }
}