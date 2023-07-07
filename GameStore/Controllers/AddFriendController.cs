using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;
using GameStore.Models.DTO;
using Newtonsoft.Json;
using Diacritics;
using Diacritics.Extensions;

namespace GameStore.Controllers
{
    public class AddFriendController : Controller
    {
        public Users Addfriend { get; private set; }
        GameStoreDataContext db= new GameStoreDataContext();
        // GET: AddFriend
        public ActionResult Index()
        {
            KHACHHANG kh = Session["KhachHang"] as KHACHHANG;
            ViewBag.kh = kh;
            return View();
        }
        [HttpGet]
        public ActionResult Search(string term)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            // Thực hiện tìm kiếm dựa trên "term" và lấy danh sách bạn bè tương ứng
            var friends = YourSearchLogic(term,kh.MaKH);

            // Trả về danh sách bạn bè dưới dạng JSON
            return Json(friends, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public List<Users> YourSearchLogic(string term, int khachHangId)
        {
            using (var db = new GameStoreDataContext())
            {
                var khachHang = db.BANBEs.FirstOrDefault(kh => kh.Id_KH == khachHangId);

                if (khachHang == null)
                {
                    if (term == "")
                    {
                        var khkhang = db.KHACHHANGs.ToList();
                        List<Users> list = new List<Users>();

                        foreach (var item in khkhang)
                        {
                            if (item.MaKH != khachHangId)
                            {
                                Users user = new Users();
                                user.MaKH = item.MaKH;
                                user.HoTen = item.HoTen;
                                list.Add(user);
                            }
                        }

                        return list;
                    }
                    else
                    {
                        var khkhang = db.KHACHHANGs.ToList();
                        List<Users> list = new List<Users>();

                        foreach (var item in khkhang)
                        {
                            string hoTen = item.HoTen.RemoveDiacritics(); // Loại bỏ dấu
                            string termWithoutDiacritics = term.RemoveDiacritics();

                            if (item.MaKH != khachHangId && hoTen.IndexOf(termWithoutDiacritics, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                Users user = new Users();
                                user.MaKH = item.MaKH;
                                user.HoTen = item.HoTen;
                                list.Add(user);
                            }
                        }

                        return list;
                    }
                }
                else
                {
                    var jsonString = khachHang.ListFriends.ToString();
                    List<Friends> listBanBe;
                    listBanBe = JsonConvert.DeserializeObject<List<Friends>>(jsonString);

                    var friendIds = listBanBe.Where(banbe => banbe.TrangThai == 0).Select(banbe => banbe.Id_banbe).ToList();

                    var users = db.KHACHHANGs.ToList();

                    List<Users> usersDTOs = new List<Users>();

                    foreach (var user in users)
                    {
                        string hoTen = user.HoTen.RemoveDiacritics(); // Loại bỏ dấu
                        string termWithoutDiacritics = term.RemoveDiacritics();

                        if (hoTen.IndexOf(termWithoutDiacritics, StringComparison.OrdinalIgnoreCase) >= 0 && user.MaKH != khachHangId && !friendIds.Contains(user.MaKH))
                        {
                            Users userDTO = new Users
                            {
                                MaKH = user.MaKH,
                                HoTen = user.HoTen
                            };
                            usersDTOs.Add(userDTO);
                        }
                    }

                    return usersDTOs;
                }

            }
        }

        [HttpPost]
        public ActionResult CheckTrangThai()
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            var banBe = db.BANBEs.FirstOrDefault(bb => bb.Id_KH == kh.MaKH);
            if (banBe != null)
            {
                var friendList = banBe.ListFriends.ToString();
                List<Friends> listBanBe = JsonConvert.DeserializeObject<List<Friends>>(friendList);
                var checkTrangThai = listBanBe.Where(n => n.TrangThai == 1).Select(n => n.Id_banbe).ToList();
                return Json(new { checkTrangThai });
            }
            return Json(new { checkTrangThai = new List<int>() }); // Trả về danh sách rỗng
        }

        [HttpPost]
        public ActionResult CreateFriendship(int friendId)
        {
            try
            {
                // Trang thai = 0 : da la ban be
                // Trang thai = 1 : Da gui loi moi ket ban
                // Trang thai = 2 : Doi xac thuc loi mooi ket ban 
                KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                var nguoigui = db.BANBEs.Where(i => i.Id_KH == kh.MaKH);
                var nguoinhan = db.BANBEs.Where(i => i.Id_KH == friendId);

                AddFriends newFriendshipNguoigui = new AddFriends()
                {
                    Id_banbe = friendId,
                    TrangThai = 1,
                    NgayGui = DateTime.Now.Date
                };

                AddFriends newFriendshipNguoinhan = new AddFriends()
                {
                    Id_banbe = kh.MaKH,
                    TrangThai = 2,
                    NgayGui = DateTime.Now.Date
                };

                // Chuyển đổi đối tượng Banbe thành chuỗi JSON
                string jsonDataNguoiGui = JsonConvert.SerializeObject(newFriendshipNguoigui);
                string jsonDataNguoiNhan = JsonConvert.SerializeObject(newFriendshipNguoinhan);

                if (nguoinhan.Count() < 1 && nguoigui.Count() < 1)
                {
                            BANBE nggui = new BANBE();
                            nggui.Id_KH = kh.MaKH;
                            nggui.ListFriends = '['+ jsonDataNguoiGui+ ']';
                            nggui.dateModifier = DateTime.Now;
                            db.BANBEs.InsertOnSubmit(nggui);

                            BANBE ngnhan = new BANBE();
                            ngnhan.Id_KH = friendId;
                            ngnhan.ListFriends ='['+jsonDataNguoiNhan +']';
                            ngnhan.dateModifier = DateTime.Now;
                            db.BANBEs.InsertOnSubmit(ngnhan);

                }
                else
                {
                    if(nguoigui.Count() == 1 && nguoinhan.Count() < 1)
                    {
                        BANBE ngnhan = new BANBE();
                        ngnhan.Id_KH = friendId;
                        ngnhan.ListFriends = '['+jsonDataNguoiNhan +']';
                        ngnhan.dateModifier = DateTime.Now;
                        db.BANBEs.InsertOnSubmit(ngnhan);

                        nguoigui.SingleOrDefault().ListFriends = '[' + nguoigui.SingleOrDefault().ListFriends.Trim('[', ']') + "," + jsonDataNguoiGui + ']';

                    }
                    if (nguoigui.Count() < 1 && nguoinhan.Count() == 1)
                    {
                        BANBE nggui = new BANBE();
                        nggui.Id_KH = kh.MaKH;
                        nggui.ListFriends = '[' + jsonDataNguoiGui + ']';
                        nggui.dateModifier = DateTime.Now;
                        db.BANBEs.InsertOnSubmit(nggui);

                        nguoinhan.SingleOrDefault().ListFriends = '[' + nguoinhan.SingleOrDefault().ListFriends.Trim('[', ']') + "," + jsonDataNguoiNhan + ']';
                    }
                    if (nguoigui.Count() == 1 && nguoinhan.Count() == 1)
                    {
                        nguoigui.SingleOrDefault().ListFriends = '[' + nguoigui.SingleOrDefault().ListFriends.Trim('[', ']') + "," + jsonDataNguoiGui +']';
                        nguoinhan.SingleOrDefault().ListFriends = '[' + nguoinhan.SingleOrDefault().ListFriends.Trim('[', ']') + "," + jsonDataNguoiNhan + ']';
                    }
                }
                 db.SubmitChanges();
                // Trả về một JSON hoặc thông báo thành công cho phía client
                return Json(new { success = 200, message = "Thêm bạn bè thành công" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi cho phía client
                return Json(new { success = 500, message = "Lỗi khi thêm bạn bè: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteFriendship(int friendId)
        {
            try
            {
                KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                var nguoigui = db.BANBEs.FirstOrDefault(i => i.Id_KH == kh.MaKH); // khách hàng 
                var nguoinhan = db.BANBEs.FirstOrDefault(i => i.Id_KH == friendId); // Người được add

                // Convert json người gửi
                var listJsonKh = nguoigui.ListFriends.ToString();
                List<AddFriends> listJsonKhConvert;
                listJsonKhConvert = JsonConvert.DeserializeObject<List<AddFriends>>(listJsonKh);

                // Convert json người nhận
                var listJsonFriend = nguoinhan.ListFriends.ToString();
                List<AddFriends> listJsonFriendConvert;
                listJsonFriendConvert = JsonConvert.DeserializeObject<List<AddFriends>>(listJsonFriend);

                if(listJsonKhConvert.Count() > 1 && listJsonFriendConvert.Count() > 1)
                {
                    var nggui = listJsonKhConvert.FindIndex(id => id.Id_banbe == friendId);
                    var ngnhan = listJsonFriendConvert.FindIndex(id => id.Id_banbe == kh.MaKH);

                    listJsonKhConvert.RemoveAt(nggui);
                    nguoigui.ListFriends = JsonConvert.SerializeObject(listJsonKhConvert);
                    listJsonFriendConvert.RemoveAt(ngnhan);
                    nguoinhan.ListFriends = JsonConvert.SerializeObject(listJsonFriendConvert);
                }
                if (listJsonKhConvert.Count() == 1 && listJsonFriendConvert.Count() == 1)
                {
                    db.BANBEs.DeleteOnSubmit(nguoigui);
                    db.BANBEs.DeleteOnSubmit(nguoinhan);
                }
                else
                {
                    if(listJsonKhConvert.Count() > 1  && listJsonFriendConvert.Count() == 1)
                    {
                        var nggui = listJsonKhConvert.FindIndex(id => id.Id_banbe == friendId);
                        listJsonKhConvert.RemoveAt(nggui);
                        nguoigui.ListFriends = JsonConvert.SerializeObject(listJsonKhConvert);

                        db.BANBEs.DeleteOnSubmit(nguoinhan);
                    }
                    if (listJsonKhConvert.Count() == 1 && listJsonFriendConvert.Count() > 1)
                    {
                        db.BANBEs.DeleteOnSubmit(nguoigui);

                        var ngnhan = listJsonFriendConvert.FindIndex(id => id.Id_banbe == kh.MaKH);
                        listJsonFriendConvert.RemoveAt(ngnhan);
                        nguoinhan.ListFriends = JsonConvert.SerializeObject(listJsonFriendConvert);
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


     
    
    }
}