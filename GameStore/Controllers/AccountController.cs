using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GameStore.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace GameStore.Controllers
{
    public class AccountController : Controller
    {
        GameStoreDataContext db = new GameStoreDataContext();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public JsonResult CheckLoginSocialAccount(string strs)
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
        [HttpGet]
        public JsonResult CheckLogin(string username, string password)
        {
            try
            {
                int check = db.KHACHHANGs.Where(s => s.TaiKhoan == username && s.MatKhau == password).Count();
                var getkh = db.KHACHHANGs.Where(s => s.TaiKhoan == username && s.MatKhau == password).SingleOrDefault();
                if (check == 1)
                {
                    Session["KhachHang"] = getkh;
                }
                else
                {                
                    return Json(new { code = 400, msg = "Lay thong tin that bai " }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = 200, msg = "Lay thong tin thanh cong." },
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

        [HttpPost]
        public JsonResult Register(string username, string password, string email,string fullname, string address)
        {
            try
            {
                // Kiểm tra các giá trị đầu vào và thực hiện xử lý
                if (string.IsNullOrEmpty(username))
                {
                    return Json(new { code = 400, msg = "Please enter your username." }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(password))
                {
                    return Json(new { code = 400, msg = "Please enter your password." }, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(email))
                {
                    return Json(new { code = 400, msg = "Please enter your email." }, JsonRequestBehavior.AllowGet);
                }
                // Kiểm tra tài khoản đã tồn tại trong cơ sở dữ liệu
                var existingUser = db.KHACHHANGs.FirstOrDefault(s => s.TaiKhoan == username);
                if (existingUser != null)
                {
                    return Json(new { code = 400, msg = "Username already exists." }, JsonRequestBehavior.AllowGet);
                }

                // Kiểm tra Email đã tồn tại trong cơ sở dữ liệu
                var existingEmail = db.KHACHHANGs.FirstOrDefault(s => s.Email == email);
                if (existingEmail != null)
                {
                    return Json(new { code = 400, msg = "Email already exists." }, JsonRequestBehavior.AllowGet);
                }

                // Thực hiện lưu thông tin vào cơ sở dữ liệu
                KHACHHANG kh = new KHACHHANG();
                kh.TaiKhoan = username;
                kh.MatKhau = password;
                kh.Email = email;
                kh.HoTen = fullname;
                kh.DiaChi = address;
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();

                Session["KhachHang"] = kh;

                return Json(new { code = 200, msg = "Register successful." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "An error occurred: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Changepassword(string recipientEmail, string code, string password)
        {
            try
            {
                var kh = db.KHACHHANGs.Where(s => s.Email == recipientEmail);
                if ( kh.Count()<1 )
                {
                    return Json(new { code = 400, msg = "Email or User Name not exists." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if(code != kh.SingleOrDefault().XacMinh)
                    {
                        return Json(new { code = 400, msg = "Invalid identification code." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        kh.SingleOrDefault().MatKhau = password;
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Thành công" });
                    }                
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult SendVerificationCode(string recipientEmail, string username)
        {
            try
            {
                // Tạo mã xác minh ngẫu nhiên gồm 6 ký tự
                string verificationCode = GenerateVerificationCode();

                // Lưu mã xác minh xuống cơ sở dữ liệu
                var kh = db.KHACHHANGs.Where(s => s.Email == recipientEmail && s.TaiKhoan== username);
                if(kh.Count()<1)
                {
                    return Json(new { code = 400, msg = "Email or User Name not exists." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var getkh = kh.SingleOrDefault();
                    getkh.XacMinh = verificationCode;
                    db.SubmitChanges();
                }

                // Gửi mã xác minh qua email
                SendEmail(recipientEmail, "Mã xác minh", "Mã xác minh của bạn là: " + verificationCode);

                return Json(new { code = 200, msg = "Mã xác nhận đã gửi" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        private string GenerateVerificationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }

            return sb.ToString();
        }


        // Hàm gửi email
        private void SendEmail(string recipientEmail, string subject, string message)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("pingvocuc555@gmail.com"); // Thay đổi email người gửi
                mail.To.Add(recipientEmail);
                mail.Subject = subject;
                mail.Body = message;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("pingvocuc555@gmail.com", "bkxflmlmnyxxrzrz"); // Thay đổi email và mật khẩu người gửi
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                   
                }
            }
        }

        public ActionResult DetailInfo()
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            var acc = db.KHACHHANGs.Where(n => n.MaKH ==kh.MaKH).Single();

            return View(acc);
        }

        [HttpPost]
        public ActionResult DetailInfo(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                var acc = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);

                if (acc != null)
                {
                    acc.HoTen = form["inputName"];
                    acc.TaiKhoan = form["inputUsername"];
                    acc.Email = form["inputEmailAddress"];
                    acc.Avatar = form["inputAvatar"];
                    acc.DiaChi = form["inputAddress"];
                    acc.DienThoai = form["inputPhone"];
                    DateTime ngaySinh;
                    if (DateTime.TryParse(form["inputBirthday"], out ngaySinh))
                    {
                        acc.NgaySinh = ngaySinh;
                    }
                    db.SubmitChanges();

                    var updatedAcc = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);

                    if (updatedAcc != null)
                    {
                        Session["KhachHang"] = updatedAcc; // Cập nhật session với thông tin mới
                        return RedirectToAction("DetailInfo");
                    }
                }

                return RedirectToAction("DetailInfo");
            }

            return View();
        }

        [HttpPost]
        public JsonResult ChangePass(string newPass, string confirmPass)
        {
            // Kiểm tra giá trị không được để trống
            if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                ModelState.AddModelError("", "Please fill in all fields.");
                return Json(new { success = false, message = "Please fill in all fields." });
            }

            // Kiểm tra new password và confirm password khớp nhau
            if (newPass != confirmPass)
            {
                ModelState.AddModelError("", "New password and confirm password do not match.");
                return Json(new { success = false, message = "New password and confirm password do not match." });
            }
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            var acc = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);

            acc.MatKhau = newPass;
            db.SubmitChanges();

            var updatedAcc = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);

            if (updatedAcc != null)
            {
                Session["KhachHang"] = updatedAcc; // Cập nhật session với thông tin mới
            }
            // Trả về phản hồi thành công
            return Json(new { success = true, message = "Password changed successfully." });
        }
        [HttpPost]
        public ActionResult UploadProfilePicture(HttpPostedFileBase profilePicture)
        {
            if (profilePicture != null && profilePicture.ContentLength > 0)
            {
                try
                {
                    // Kiểm tra kiểu tệp tin ảnh
                    if (!IsImageFile(profilePicture))
                    {
                        return Json(new { success = false, message = "Invalid image file. Please upload a JPEG or PNG file." });
                    }

                    // Kiểm tra kích thước tệp tin ảnh
                    if (!IsFileSizeValid(profilePicture))
                    {
                        return Json(new { success = false, message = "Image file size exceeds the limit. Please upload an image file no larger than 5 MB." });
                    }

                    // Tạo tên tệp tin duy nhất
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Theme/img"), fileName);
                    profilePicture.SaveAs(filePath);

                    // Cập nhật đường dẫn ảnh hồ sơ trong cơ sở dữ liệu (theo mã khách hàng của bạn)
                    KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                    var acc = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);
                    acc.Avatar = $"/Theme/img/{fileName}"; // Lưu đường dẫn dựa trên src của thẻ <img>
                    db.SubmitChanges();

                    var updatedAcc = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == kh.MaKH);

                    if (updatedAcc != null)
                    {
                        Session["KhachHang"] = updatedAcc; // Cập nhật session với thông tin mới
                        return RedirectToAction("DetailInfo");
                    }

                    return Json(new { success = true, message = "Profile picture updated successfully.", imageUrl = $"~/Theme/img/{fileName}" });
                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "An error occurred while uploading the profile picture. Please try again." });
                }
            }

            return Json(new { success = false, message = "No image file selected." });
        }

        // Kiểm tra kiểu tệp tin ảnh
        private bool IsImageFile(HttpPostedFileBase file)
        {
            return file.ContentType.StartsWith("image/");
        }

        // Kiểm tra kích thước tệp tin ảnh (giới hạn là 5 MB)
        private bool IsFileSizeValid(HttpPostedFileBase file)
        {
            const int maxFileSize = 5 * 1024 * 1024; // 5 MB
            return file.ContentLength <= maxFileSize;
        }

        // Tạo tên tệp tin duy nhất dựa trên timestamp
        private string GenerateFileName(HttpPostedFileBase file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $"{fileName}_{timestamp}{fileExtension}";
        }

    }

}