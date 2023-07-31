using GameStore.Models;
using GameStore.Models.DAO;
using GameStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store   
     
        public ActionResult Store(int page = 1, int pageSize = 21)
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

        public ActionResult SearchGame(string searchQuery,int page = 1, int pageSize = 21)
        {
            int totalGames = ConstServer.Instance.TongSoLuongGame;

            // Tính toán số trang dựa trên số lượng sản phẩm và kích thước trang
            int totalPages = (int)Math.Ceiling((double)totalGames / pageSize);

            // Perform the search based on the searchQuery
            List<Games> searchResults;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Assuming you have a method to search for games by the given query in your GamesDAO
                searchResults = GamesDAO.Instance.GetGameBySearch(searchQuery,page, pageSize);
            }
            else
            {
                // If the search query is empty, display all games (or a default list of games)
                searchResults = GamesDAO.Instance.GetGameByPage(page, pageSize);
            }

            // Truyền danh sách sản phẩm tìm kiếm và thông tin phân trang đến View
            ViewBag.Products = searchResults;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View("Store", searchResults); // Assuming you have a view called "Store" to display the search results.       
        }

            public ActionResult Store10Seller()
        {
            DataTable gameList = DataProvider.Instance.ExcuteQuery("SELECT TOP 10 * FROM Game ORDER BY numSale DESC");

            
            List<Games> games = new List<Games>();

            foreach (DataRow row in gameList.Rows)
            {
                Games game = new Games(row);
                games.Add(game);
            }

            return PartialView(games);
        }

        public ActionResult Store4Feature()
        {
            DataTable gameList = DataProvider.Instance.ExcuteQuery("SELECT TOP 4 * FROM Game ORDER BY priceGame DESC");

            List<Games> games = new List<Games>();

            foreach (DataRow row in gameList.Rows)
            {
                Games game = new Games(row);
                games.Add(game);
            }

            return PartialView(games);
        }

        public ActionResult GameDetail(int id)
        {
            var query = $"SELECT TOP 1 * FROM Game WHERE id = {id}";
            var result = new Games(DataProvider.Instance.ExcuteQuery(query).Rows[0]);
            return View(result);
        }


    }
}