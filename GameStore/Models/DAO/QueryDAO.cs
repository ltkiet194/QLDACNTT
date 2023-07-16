using GameStore.Models.DTO.GameModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GameStore.Models.DAO
{
    public class QueryDAO
    {

        private static QueryDAO instance;

        public static QueryDAO Instance
        {
            get
            {
                if (instance == null)
                {

                    instance = new QueryDAO();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public List<Category> LoadTheLoai()
        {
            var result = new List<Category>();
            DataTable dt = DataProvider.Instance.ExcuteQuery("SELECT * FROM THELOAI");
            foreach(DataRow row in dt.Rows)
            {
                Category cate = new Category(row);
                result.Add(cate);
            }
            return result;
        }

        
    }
}