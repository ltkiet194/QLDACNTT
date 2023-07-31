using GameStore.Models.DTO;
using GameStore.Models.DTO.GameModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GameStore.Models.DAO
{
    public class GamesDAO
    {
        private static GamesDAO instance;

        public static GamesDAO Instance
        {
            get
            {
                if (instance == null)
                {

                    instance = new GamesDAO();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }


        public List<Games> LoadGameRelate(string typegame)
        {
            var result = new List<Games>();
            DataTable dt = DataProvider.Instance.ExcuteQuery("SELECT TOP 6 * FROM Game WHERE typeGame LIKE N'%"+ typegame + "%'");
            foreach (DataRow row in dt.Rows)
            {
                Games cate = new Games(row);
                result.Add(cate);
            }
            return result;
        }

        public List<Games> GetGameByPage(int page ,int pagesize)
        {
            var result = new List<Games>();
            DataTable dt = DataProvider.Instance.ExcuteQuery($"SELECT * FROM Game ORDER BY Id OFFSET ({page} - 1) * {pagesize} ROWS FETCH NEXT {pagesize} ROWS ONLY;");
            foreach (DataRow row in dt.Rows)
            {
                Games cate = new Games(row);
                result.Add(cate);
            }
            return result;
        }

        public List<Games> GetGameBySearch(string query, int page, int pageSize)
        {
            var result = new List<Games>();
            DataTable dt = DataProvider.Instance.ExcuteQuery($"SELECT * FROM Game WHERE nameGame LIKE '%{query}%' ORDER BY Id OFFSET ({page} - 1) * {pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY;");
            foreach (DataRow row in dt.Rows)
            {
                Games game = new Games(row);
                result.Add(game);
            }
            return result;
        }
        public int GetNumGameBySearch(string query)
        {
            var result = new List<Games>();
            DataTable dt = DataProvider.Instance.ExcuteQuery($"SELECT * FROM Game WHERE nameGame LIKE '%{query}%' ");
            foreach (DataRow row in dt.Rows)
            {
                Games game = new Games(row);
                result.Add(game);
            }
            return result.Count();
        }

        public Games GetGameById(int id)
        {          
            DataTable dt = DataProvider.Instance.ExcuteQuery($"SELECT * FROM Game WHERE Id = {id}");     
            return new Games(dt.Rows[0]);
        }
        public int CheckExistGameCommentById(int id)
        {         
            return DataProvider.Instance.ExcuteQuery($"SELECT * FROM DANHGIA WHERE Id_Game = {id}").Rows.Count;
        }


        public string GetCommentLisStringtById(int id)
        {
            string result = "[]";
            if (DataProvider.Instance.ExcuteQuery($"SELECT * FROM DANHGIA WHERE Id_Game = {id}").Rows.Count>0)
            {
                result = DataProvider.Instance.ExcuteQuery($"SELECT * FROM DANHGIA WHERE Id_Game = {id}").Rows[0][2].ToString();
            }
            return result;
        }
        public void SaveCommentListById(int id, string listComment)
        {
            DataProvider.Instance.ExcuteNonQuery($"UPDATE DANHGIA SET ListComment = N'{listComment}' WHERE Id_Game = {id}");
        }

        public void InsertCommentList(int id,string listComment)
        {
            DataProvider.Instance.ExcuteNonQuery($"INSERT INTO DANHGIA (Id_Game, ListComment, NgayTao) VALUES ({id}, N'{listComment}', GETDATE())");
        }


        public string GetGameCartByIdKH(int id)
        {
            DataRow row = DataProvider.Instance.ExcuteQuery($"SELECT * FROM KHACHHANG WHERE MaKH = {id}").Rows[0];
            string list = row["ListGameInCart"].ToString();
            if(list==null || list.Length == 0)
            {
                list = "[]";
            }
            return list;
        }

        public void SaveGameInCartListByIdKH(int idkh, string listGame)
        {
            DataProvider.Instance.ExcuteNonQuery($"UPDATE KHACHHANG SET ListGameInCart = N'{listGame}' WHERE MaKH = {idkh}");
        }

    }
}