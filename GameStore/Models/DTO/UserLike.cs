using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO
{
    public class UserLike
    {
        public int Id_kh { get; set; }
        public int Id_Bd { get; set; }
        public DateTime NgayLike { get; set; }   
    }
}