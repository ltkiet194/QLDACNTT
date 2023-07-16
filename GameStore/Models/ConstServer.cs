using GameStore.Models.DAO;
using GameStore.Models.DTO;
using GameStore.Models.DTO.GameModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models
{
    public class ConstServer
    {
        private static ConstServer instance;
        GameStoreDataContext data = new GameStoreDataContext();
        public static ConstServer Instance
        {
            get
            {
                if (instance == null)
                {

                    instance = new ConstServer();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        public int TongSoLuongGame = 0;

        public ConstServer()
        {
            TongSoLuongGame = data.Games.Count();

        }

        public static List<Category> listCategory = QueryDAO.Instance.LoadTheLoai();



   
        



    }
}