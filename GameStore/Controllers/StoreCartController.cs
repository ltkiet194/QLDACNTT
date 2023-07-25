using GameStore.Models;
using GameStore.Models.DAO;
using GameStore.Models.DTO;
using GameStore.Models.DTO.GameModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace GameStore.Controllers
{
    public class StoreCartController : Controller
    {
        // GET: StoreCart
        GameStoreDataContext db = new GameStoreDataContext();
        public ActionResult StoreCart()
        {
            return View();
        }
        public ActionResult AddToCart(int gameId, string gameName ,string imgName, int priceGame, string url)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            List<GamesInCart> games = JsonConvert.DeserializeObject<List<GamesInCart>>(GamesDAO.Instance.GetGameCartByIdKH(kh.MaKH));
            GamesInCart cartItem = games.FirstOrDefault(g => g.Id == gameId);
            if (cartItem == null)
            {
                // If the game is not in the shopping cart, create a new GamesInCart item
                cartItem = new GamesInCart(gameId, gameName, imgName,1, priceGame);   
                games.Add(cartItem);
            }
            else
            {
                // If the game is already in the shopping cart, increase its quantity by 1
                cartItem.Quantity += 1;
            }
            string json = JsonConvert.SerializeObject(games);

            GamesDAO.Instance.SaveGameInCartListByIdKH(kh.MaKH, json);

            return Redirect(url);
        }

        public ActionResult RemoveToCart(int gameId, string url)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            List<GamesInCart> games = JsonConvert.DeserializeObject<List<GamesInCart>>(GamesDAO.Instance.GetGameCartByIdKH(kh.MaKH));

            GamesInCart cartItem = games.FirstOrDefault(g => g.Id == gameId);
            if (cartItem != null)
            {
                games.Remove(cartItem);
            }
            string json = JsonConvert.SerializeObject(games);

            GamesDAO.Instance.SaveGameInCartListByIdKH(kh.MaKH, json);

            return Redirect(url);
        }

        [HttpPost]
        public ActionResult UpdateQuantity(FormCollection f)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            List<GamesInCart> games = JsonConvert.DeserializeObject<List<GamesInCart>>(GamesDAO.Instance.GetGameCartByIdKH(kh.MaKH));

            foreach (var key in f.AllKeys)
            {               
                if (key.StartsWith("quantity"))
                {
                    var id = key.Replace("quantity_", "");
                    GamesInCart cartItem = games.FirstOrDefault(g => g.Id == int.Parse(id));
                    var quality = f[key];
                    cartItem.Quantity = int.Parse(quality);
                }
            }
            string json = JsonConvert.SerializeObject(games);

            GamesDAO.Instance.SaveGameInCartListByIdKH(kh.MaKH, json);

            return RedirectToAction("StoreCart","StoreCart");
        }
        [HttpPost]
        public JsonResult BuyCart(string total)
        {
            try
            {
                KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
                
                var GetKH = db.KHACHHANGs.SingleOrDefault(k => k.MaKH == kh.MaKH);
                var LuuListGame = GetKH.ListGameInCart;
                List<GamesInCart> games = JsonConvert.DeserializeObject<List<GamesInCart>>(LuuListGame);
                Random random = new Random();
                foreach (var game in games)
                {
                    for (int i = 0; i < game.Quantity; i++)
                    {
                        string key = GenerateRandomKey(random, 10); // Call a function to generate a random key
                        KEYGAME keyGame = new KEYGAME
                        {
                            ID_KH = kh.MaKH,
                            NameGame = game.Name,
                            KeyGames = key,
                            DateModified = DateTime.Now,
                        };

                        // Add the generated key to the database
                        db.KEYGAMEs.InsertOnSubmit(keyGame);
                        var existingGame = games.SingleOrDefault(g => g.Id == game.Id);
                        if (existingGame != null)
                        {
                            existingGame.Quantity += 1; 
                        }
                        else
                        {
                            games.Add(new GamesInCart
                            {
                                Id = game.Id,
                                Name = game.Name,
                                ImgName = game.ImgName,
                                Quantity = 1,
                                PriceGame = game.PriceGame
                            });
                        }
                    }
                }
                
                GetKH.ListGameInLibrary = JsonConvert.SerializeObject(games);
                
                if (GetKH.Balance < float.Parse(total))
                {
                    return Json(new { success = false, message = "Bạn không đủ tiền trong tài khoản !"});
                }
                else
                {
                    DONHANG donhang = new DONHANG
                    {
                        Id_KH = kh.MaKH,
                        NgayMua = DateTime.Now,
                        TinhTrang = true,
                        Id_PTThanhToan = 1,
                        TongTien = float.Parse(total),
                    };

                    db.DONHANGs.InsertOnSubmit(donhang);
                    GetKH.ListGameInCart = null;
                    
                    GetKH.Balance = GetKH.Balance - float.Parse(total);
                    db.SubmitChanges();
                    Session["KhachHang"] = db.KHACHHANGs.Where(m=>m.MaKH == kh.MaKH).SingleOrDefault();
                    return Json(new { success = true, message = "Mua thành công." });
                }
            }
            catch(Exception ex)
            {

                return Json(new { success = false, message = "Đã xảy ra lỗi khi mua hàng: " + ex.Message });
            }
        }
        private string GenerateRandomKey(Random random, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}