using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Areas.Admin
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgName { get; set; }
        public string ListImage { get; set; }
        public string AttGame { get; set; }
        public string TagGame { get; set; }
        public string TypeGame { get; set; }
        public string DevsGame { get; set; }
        public string Publisher { get; set; }
        public double? PriceGame { get; set; }
        public string DesGame { get; set; }
        public int? YearRelease { get; set; }
        public string RequireGame { get; set; }
        public int? NumSale { get; set; }
        public bool? EnableGame { get; set; }
        public DateTime? DateModified { get; set; }
    }
}