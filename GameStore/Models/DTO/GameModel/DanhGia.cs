using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO.GameModel
{
    public class DanhGia
    {
        public int Id { get; set; }
        public int Id_Game { get; set; }
        public List<Comment> ListComment { get; set; }
        public DateTime NgayTao { get; set; }

        public DanhGia(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            Id_Game = Convert.ToInt32(row["Id_Game"].ToString());
            //ListComment = row["ListComment"].ToString();
            NgayTao = Convert.ToDateTime( row["listImage"].ToString());            
        }

    }
}