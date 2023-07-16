using GameStore.Models;
using GameStore.Models.DAO;
using GameStore.Models.DTO.GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace GameStore.Controllers
{
    public class StoreProductController : Controller
    {
        // GET: StoreProduct
        GameStoreDataContext data = new GameStoreDataContext();
        public ActionResult StoreProduct()
        {
            ViewBag.TrangChu = 1;
            ViewBag.StoreProduct = 1;
            return View();
        }
        public ActionResult PartialCategory()
        {
            var listCate = data.THELOAIs.ToList();
            return PartialView(listCate);
        }
        public ActionResult PartialPriceFilter()
        {
            return PartialView();
        }
        public ActionResult PartialGameEnviroment()
        {
            // PC or PS or Xbox..
            return PartialView();
        }

        [HttpPost]
        public ActionResult ReviewGame(int GameId,string Message, string Rating)
        {
            KHACHHANG kh = (KHACHHANG)Session["KhachHang"];
            List<Comment> listcmt= new List<Comment>();
               
            if (GamesDAO.Instance.CheckExistGameCommentById(GameId) == 0)
            {
                Comment cmt = new Comment(0, kh.MaKH, Message, int.Parse(Rating), kh.HoTen);
                listcmt.Add(cmt);
                string json = JsonConvert.SerializeObject(listcmt);
                GamesDAO.Instance.InsertCommentList(GameId, json);

            }
            else
            {
                listcmt = JsonConvert.DeserializeObject<List<Comment>>(GamesDAO.Instance.GetCommentLisStringtById(GameId));
                Comment cmt = new Comment(listcmt.Count, kh.MaKH, Message, int.Parse(Rating), kh.HoTen);
                listcmt.Add(cmt);
                string json = JsonConvert.SerializeObject(listcmt);
                GamesDAO.Instance.SaveCommentListById(GameId, json);
            }
            return Json(new { success = true });
        }

    }
}