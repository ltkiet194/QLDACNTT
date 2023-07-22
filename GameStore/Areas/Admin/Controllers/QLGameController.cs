using GameStore.Models.DAO;
using GameStore.Models.DTO;
using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace GameStore.Areas.Admin.Controllers
{
    public class QLGameController : Controller
    {
        GameStoreDataContext db = new GameStoreDataContext();

        // GET: Admin/QLGame
        public ActionResult QLGames(int page = 1, int pageSize = 21)
        {

            //int products = GetProductsFromDataSource();

            // Tính toán số trang dựa trên số lượng sản phẩm và kích thước trang
            int totalGames = ConstServer.Instance.TongSoLuongGame;
            int totalPages = (int)Math.Ceiling((double)totalGames / pageSize);

            // Lấy các sản phẩm cho trang hiện tại
            List<Games> productsForPage = GamesDAO.Instance.GetGameByPage(page, pageSize);

            // Truyền danh sách sản phẩm và thông tin phân trang đến View
            ViewBag.Products = productsForPage;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(productsForPage);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/QLGame/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection formData, HttpPostedFileBase imgGame)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tính hợp lệ của các trường dữ liệu
                if (string.IsNullOrEmpty(formData["nameGame"]))
                {
                    ModelState.AddModelError("nameGame", "Vui lòng nhập tên game.");
                    return View();
                }

                if (string.IsNullOrEmpty(formData["tagGame"]))
                {
                    ModelState.AddModelError("tagGame", "Vui lòng nhập tag game.");
                    return View();
                }

                if (string.IsNullOrEmpty(formData["typeGame"]))
                {
                    ModelState.AddModelError("typeGame", "Vui lòng nhập loại game.");
                    return View();
                }
                if (string.IsNullOrEmpty(formData["devsGame"]))
                {
                    ModelState.AddModelError("devsGame", "Vui lòng nhập đầy đủ thông tin.");
                    return View();
                }
                if (string.IsNullOrEmpty(formData["publisher"]))
                {
                    ModelState.AddModelError("publisher", "Vui lòng nhập đầy đủ thông tin.");
                    return View();
                }
                if (string.IsNullOrEmpty(formData["priceGame"]))
                {
                    ModelState.AddModelError("priceGame", "Vui lòng nhập đầy đủ thông tin.");
                    return View();
                }
                if (string.IsNullOrEmpty(formData["desGame"]))
                {
                    ModelState.AddModelError("desGame", "Vui lòng nhập đầy đủ thông tin.");
                    return View();
                }
                if (string.IsNullOrEmpty(formData["yearRelease"]))
                {
                    ModelState.AddModelError("yearRelease", "Vui lòng nhập đầy đủ thông tin.");
                    return View();
                }
                if (string.IsNullOrEmpty(formData["requireGame"]))
                {
                    ModelState.AddModelError("requireGame", "Vui lòng nhập đầy đủ thông tin.");
                    return View();
                }
                // Lưu hình ảnh vào thư mục trên máy chủ (ví dụ: ~/Images/Games)
                if (imgGame != null && imgGame.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(imgGame.FileName);
                    string path = Path.Combine(Server.MapPath("~/Theme/img/"), fileName);
                    imgGame.SaveAs(path);

                    // Lưu đường dẫn của hình ảnh vào formData
                    formData["imgGame"] = "~/Theme/img/" + fileName;
                }

                // Tạo đối tượng Game từ formData
                Game newGame = new Game
                {
                    nameGame = formData["nameGame"],
                    imgName = formData["imgGame"],
                    tagGame = formData["tagGame"],
                    typeGame = formData["typeGame"],
                    devsGame = formData["devsGame"],
                    publisher = formData["publisher"],
                    priceGame = float.Parse(formData["priceGame"]),
                    desGame = formData["desGame"],
                    yearRelease = int.Parse(formData["yearRelease"]),
                    requireGame = formData["requireGame"]
                };

                // Thêm đối tượng newGame vào cơ sở dữ liệu bằng LINQ to SQL
                db.Games.InsertOnSubmit(newGame);
                db.SubmitChanges();

                // Chuyển hướng về trang danh sách sau khi thêm thành công
                return RedirectToAction("QLGames", "QLGame");
            }

            // Nếu ModelState không hợp lệ, trả về trang Create để người dùng nhập lại thông tin
            return View();
        }
        public ActionResult Edit(int id)
        {

            return View(db.Games.Where(m=>m.id == id).SingleOrDefault());
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            // Tìm Game theo Id
            var gameToDelete = db.Games.SingleOrDefault(g => g.id == id);

            try
            {
                // Xóa Game khỏi CSDL
                db.Games.DeleteOnSubmit(gameToDelete);
                db.SubmitChanges();

                // Trả về kết quả thành công
                return Json(new { success = true, message = "Xóa Game thành công." });
            }
            catch (Exception ex)
            {
                // Nếu xảy ra lỗi, trả về thông báo lỗi
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa Game: " + ex.Message });
            }
        }
        [HttpPost]
        public JsonResult EditManage(int id,int soluongconlai)
        {
            try
            {
                var gameManage = db.Khos.SingleOrDefault(g => g.Id_game == id);
                gameManage.SoLuongConLai= soluongconlai;
                
                db.SubmitChanges();

                // Trả về kết quả thành công
                return Json(new { success = true, message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
                // Nếu xảy ra lỗi, trả về thông báo lỗi
                return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật: " + ex.Message });
            }
        }
        public JsonResult GetManage(int id)
        {
            try
            {
                // Tìm Game theo Id_game
                var gameManage = db.Khos.SingleOrDefault(g => g.Id_game == id);

                // Trả về kết quả thành công
                return Json(new { success = true, message = "Lấy Game thành công.", getsp = gameManage });
            }
            catch (Exception ex)
            {
                // Nếu xảy ra lỗi, trả về thông báo lỗi
                return Json(new { success = false, message = "Đã xảy ra lỗi khi lấy Game: " + ex.Message });
            }
        }
    }
}