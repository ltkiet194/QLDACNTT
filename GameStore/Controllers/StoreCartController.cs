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

namespace GameStore.Controllers
{
    public class StoreCartController : Controller
    {
        // GET: StoreCart
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
    }
}