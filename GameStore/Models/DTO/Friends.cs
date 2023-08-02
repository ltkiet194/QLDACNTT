using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO
{
    public class Friends

    {
        public int Id_banbe { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayGui { get; set; }

        public string Tenbanbe { get; set; }
        public string Avatar { get; set; }
        public bool Online { get; set; }
    }
}