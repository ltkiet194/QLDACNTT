using GameStore.Models.DTO;
using GameStore.Models.DTO.GameModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GameStore.Models;
using System.Text.Json;

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

        GameStoreDataContext db = new GameStoreDataContext();
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
        public List<Games> GetNumGameBySearch(string query)
        {
            var result = new List<Games>();
            DataTable dt = DataProvider.Instance.ExcuteQuery($"SELECT * FROM Game WHERE nameGame LIKE '%{query}%' ");
            foreach (DataRow row in dt.Rows)
            {
                Games game = new Games(row);
                result.Add(game);
            }
            return result;
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

        public List<TBAddFriend> GetSLBanbechuakb(KHACHHANG kh)
        {
            if (kh != null)
            {
                var listbb = db.BANBEs.Where(n => n.Id_KH == kh.MaKH).SingleOrDefault();
                if (listbb != null)
                {
                    var lisJsonFriend = listbb.ListFriends;
                    List<TBAddFriend> fr = JsonSerializer.Deserialize<List<TBAddFriend>>(lisJsonFriend);
                    var listChuaKB = fr.Where(n => n.TrangThai == 2).Select(n => n.Id_banbe).ToList();
                    var listID = fr.Where(f => listChuaKB.Contains(f.Id_banbe)).ToList();
                    foreach (var i in listID)
                    {
                        var layid = db.KHACHHANGs.FirstOrDefault(n => n.MaKH == i.Id_banbe);
                        if (layid != null)
                        {
                            i.Id_banbe = layid.MaKH;
                            i.Tenbanbe = layid.HoTen;
                        }         
                    }
                    return listID;
                } 
            }
            return new List<TBAddFriend>();
        }

    }
}