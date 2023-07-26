using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO
{
    public class Statistical
    {
        public string NameKH { get; set; }
        public string Namegame { get; set; }
        public int Quantity { get; set; }
        public float PriceGame { get; set; }
        public float Total { get; set; }

    }
}