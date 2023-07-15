using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO.GameModel
{
  

    public class Category
    {
        public int Id { get; set; }
        public string TenTheLoai { get; set; }
        public DateTime DateTime { get; set; }

        public Category(DataRow row)
        {
            Id = Convert.ToInt32(row["Id"]);
            TenTheLoai = row["TenTheLoai"].ToString();
            DateTime = Convert.ToDateTime(row["DateModified"].ToString());       
        }


    }
}