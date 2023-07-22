using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;
using GameStore.Models.DTO;
using System.Text.Json;
using FireSharp.Interfaces;
using FireSharp.Config;


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
        public ActionResult ModalChat()
        {
            return View();
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
            if (kh != null)
            {
                var listbb = db.BANBEs.Where(n => n.Id_KH == kh.MaKH).SingleOrDefault();
                if (listbb != null)
                {
                    var listjsonFriend = listbb.ListFriends;
                    List<Friends> fr = JsonSerializer.Deserialize<List<Friends>>(listjsonFriend);
                    var listfriendLocTT = fr.Where(n => n.TrangThai == 0).Select(n => n.Id_banbe).ToList();
                    var listfriendLocTT2 = fr.Where(n => n.TrangThai == 2).Select(n => n.Id_banbe).ToList();

                    var listfriendDaloc = fr.Where(f => listfriendLocTT.Contains(f.Id_banbe)).ToList();
                    ViewBag.SL = listfriendLocTT2.Count();
                    foreach (var i in listfriendDaloc)
                    {
                        var listInfoKH = db.KHACHHANGs.FirstOrDefault(n => n.MaKH == i.Id_banbe);
                        if (listInfoKH != null)
                        {
                            i.Tenbanbe = listInfoKH.HoTen;
                        }
                    }

                    return PartialView(listfriendDaloc);
                }
            }
            return PartialView();

        }

        public ActionResult PartialLi()
        {
            return PartialView("_PartialLi");
        }
        [HttpGet]
        public ActionResult AccessAddFriend()
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            if(kh != null)
            {
                var listbb = db.BANBEs.Where(n => n.Id_KH == kh.MaKH).SingleOrDefault();
                if (listbb != null)
                {
                    var lisJsonFriend = listbb.ListFriends;
                    List<TBAddFriend> fr = JsonSerializer.Deserialize<List<TBAddFriend>>(lisJsonFriend);
                    var listChuaKB = fr.Where(n => n.TrangThai == 2).Select(n => n.Id_banbe).ToList();
                    var listID = fr.Where(f => listChuaKB.Contains(f.Id_banbe)).ToList();
                   /* int countSL = 0;*/ //Dem so luong
                    foreach (var i in listID)
                    {
                        var layid = db.KHACHHANGs.FirstOrDefault(n => n.MaKH == i.Id_banbe);
                        if (layid != null)
                        {
                            i.Id_banbe = layid.MaKH;
                            i.Tenbanbe = layid.HoTen;
                            //countSL++;
                        }
                    }
                    //ViewBag.Count = countSL;
                    return PartialView(listID);
                }
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult DongYKetBan(int friendId)
        {
            try
            {
                KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                var nguoichapnhan = db.BANBEs.Where(i => i.Id_KH == kh.MaKH).SingleOrDefault(); //Thằng duyệt
                var nguoiduocchapnhan = db.BANBEs.Where(i => i.Id_KH == friendId).SingleOrDefault(); //Thằng được duyệt

                //Thằng duyệt
                var listBB1 = nguoichapnhan.ListFriends;
                List<AddFriends> fr1 = JsonSerializer.Deserialize<List<AddFriends>>(listBB1);
                var idduyet = fr1.SingleOrDefault(i => i.Id_banbe == friendId);

                //Thằng được duyệt
                var listBB2 = nguoiduocchapnhan.ListFriends;
                List<AddFriends> fr2 = JsonSerializer.Deserialize<List<AddFriends>>(listBB2);
                var idduocduyet = fr2.SingleOrDefault(i => i.Id_banbe == kh.MaKH);

                if (idduocduyet != null && idduyet != null)
                {
                    idduyet.TrangThai = 0;
                    listBB1 = JsonSerializer.Serialize(fr1);//Conver lại 
                    nguoichapnhan.ListFriends = listBB1;

                    idduocduyet.TrangThai = 0;
                    listBB2 = JsonSerializer.Serialize(fr2);//Conver lại 
                    nguoiduocchapnhan.ListFriends = listBB2;
                }

                db.SubmitChanges();
                return Json(new { success = 200, message = "Thêm bạn bè thành công" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi cho phía client
                return Json(new { success = 500, message = "Lỗi khi thêm bạn bè! " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult TuChoiKetBan(int friendId)
        {
            try
            {
                KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                var nguoigui = db.BANBEs.FirstOrDefault(i => i.Id_KH == kh.MaKH); // Thằng đăng nhập
                var nguoinhan = db.BANBEs.FirstOrDefault(i => i.Id_KH == friendId); //Nguoi bi tu choi

                // Convert json người gửi
                var listJsonKh = nguoigui.ListFriends.ToString();
                List<AddFriends> listJsonKhConvert;
                listJsonKhConvert = JsonSerializer.Deserialize<List<AddFriends>>(listJsonKh);

                // Convert json người nhận
                var listJsonFriend = nguoinhan.ListFriends.ToString();
                List<AddFriends> listJsonFriendConvert;
                listJsonFriendConvert = JsonSerializer.Deserialize<List<AddFriends>>(listJsonFriend);

                if (listJsonKhConvert.Count() > 1 && listJsonFriendConvert.Count() > 1)
                {
                    var nggui = listJsonKhConvert.FindIndex(id => id.Id_banbe == friendId);
                    var ngnhan = listJsonFriendConvert.FindIndex(id => id.Id_banbe == kh.MaKH);

                    listJsonKhConvert.RemoveAt(nggui);
                    nguoigui.ListFriends = JsonSerializer.Serialize(listJsonKhConvert);
                    listJsonFriendConvert.RemoveAt(ngnhan);
                    nguoinhan.ListFriends = JsonSerializer.Serialize(listJsonFriendConvert);
                }
                if (listJsonKhConvert.Count() == 1 && listJsonFriendConvert.Count() == 1)
                {
                    db.BANBEs.DeleteOnSubmit(nguoigui);
                    db.BANBEs.DeleteOnSubmit(nguoinhan);
                }
                else
                {
                    if (listJsonKhConvert.Count() > 1 && listJsonFriendConvert.Count() == 1)
                    {
                        var nggui = listJsonKhConvert.FindIndex(id => id.Id_banbe == friendId);
                        listJsonKhConvert.RemoveAt(nggui);
                        nguoigui.ListFriends = JsonSerializer.Serialize(listJsonKhConvert);

                        db.BANBEs.DeleteOnSubmit(nguoinhan);
                    }
                    if (listJsonKhConvert.Count() == 1 && listJsonFriendConvert.Count() > 1)
                    {
                        db.BANBEs.DeleteOnSubmit(nguoigui);

                        var ngnhan = listJsonFriendConvert.FindIndex(id => id.Id_banbe == kh.MaKH);
                        listJsonFriendConvert.RemoveAt(ngnhan);
                        nguoinhan.ListFriends = JsonSerializer.Serialize(listJsonFriendConvert);
                    }
                }
                db.SubmitChanges();
                return Json(new { success = 200, message = "Xóa bạn bè thành công" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi cho phía client
                return Json(new { success = 500, message = "Lỗi khi xóa bạn bè: " + ex.Message });
            }
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

        public JsonResult getdatachat(string id)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            if (id == null && kh == null || id == null && kh != null)
            {
                return Json(null); // Trả về một JSON trống nếu không có dữ liệu
            }
            else
            {
                string authen = "UEX3DWXKythkhw9EfQTi6NO3jyahnKbTZA2gOPic";
                string path = "https://chat-project-85651-default-rtdb.firebaseio.com/";
                IFirebaseClient client;
                IFirebaseConfig config = new FirebaseConfig
                {
                    AuthSecret = authen,
                    BasePath = path
                };
                client = new FireSharp.FirebaseClient(config);

                var response = client.Get("doc/");
                var dataFromFirebase = response.ResultAs<Dictionary<string, Dictionary<string, object>>>();
                var dataList = new List<Dictionary<string, object>>();

                foreach (var item in dataFromFirebase)
                {
                    var key = item.Key;
                    var dataItem = item.Value;

                    if (dataItem.ContainsKey("Name") && dataItem.ContainsKey("Message"))
                    {
                        var name = dataItem["Name"].ToString();
                        var message = dataItem["Message"].ToString();
                        var room = dataItem["Room"].ToString();
                        if ((name == id && id != null) || (kh != null && name == kh.MaKH.ToString()) && room == kh.MaKH + "-" + id || room == id + "-" + kh.MaKH)
                        {
                            var dataDict = new Dictionary<string, object>();
                            dataDict["Key"] = key;
                            dataDict["Name"] = name;
                            dataDict["Message"] = message;
                            dataDict["Room"] = room;

                            dataList.Add(dataDict);
                        }
                    }
                }
                return Json(dataList, JsonRequestBehavior.AllowGet); // Trả về dữ liệu dạng JSON
            }
        }

        [HttpPost]
        public JsonResult sendChat(string message, string id)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            string authen = "UEX3DWXKythkhw9EfQTi6NO3jyahnKbTZA2gOPic";
            string path = "https://chat-project-85651-default-rtdb.firebaseio.com/";
            IFirebaseClient client;
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = authen,
                BasePath = path
            };
            client = new FireSharp.FirebaseClient(config);
            if (client != null && !string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(authen))
            {
                var data = new
                {
                    Name = kh.MaKH,
                    Message = message,
                    Room = kh.MaKH + "-" + id
                };
                client.Push("doc/", data);
            }
            return Json(new { code = 200 },
                       JsonRequestBehavior.AllowGet); // Trả về kết quả dưới dạng JSON
        }
    }
}