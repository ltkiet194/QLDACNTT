using GameStore.Models.DAO;
using GameStore.Models.DTO.GameModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO
{
    public class Games
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgName { get; set; }
        public string ListImage { get; set; }
        public string AttGame { get; set; }
        public string[] TagGame { get; set; }
        public string[] TypeGame { get; set; }
        public string[] DevsGame { get; set; }
        public string[] Publisher { get; set; }
        public float PriceGame { get; set; }
        public string DesGame { get; set; }
        public int YearRelease { get; set; }
        public string RequireGame { get; set; }
        public int NumSale { get; set; }
        public bool EnableGame { get; set; }
        public List<Comment> ListComment { get; set; }
        public DateTime DateModified { get; set; }

        public float AverageRating = 0;
      
        public Games(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            Name = row["nameGame"].ToString();
            ImgName = row["imgName"].ToString();
            ListImage = row["listImage"].ToString();
            AttGame = row["attGame"].ToString();
            TagGame = row["tagGame"].ToString().Replace("'", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Split(',');
            TypeGame = row["typeGame"].ToString().Replace("'", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Split(',');       
                DevsGame = row["devsGame"].ToString().Replace("'", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Split(','); 
                Publisher = row["publisher"].ToString().Replace("'", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Split(','); 
            PriceGame = Convert.ToSingle(row["priceGame"]);
            DesGame = row["desGame"].ToString();
            YearRelease = Convert.ToInt32(row["yearRelease"]);
            RequireGame = row["requireGame"].ToString()
    .Replace("https://taigame.org/misc/helpPlay#system-requirements", "#")
    .Replace("<h4>Cấu hình tối thiểu</h4>", "<h4 style=\"color:#062c33\">Cấu hình tối thiểu</h4>");

            NumSale = Convert.ToInt32(row["numSale"]);
            EnableGame = Convert.ToBoolean(row["enableGame"]);

            ListComment = JsonConvert.DeserializeObject<List<Comment>>(GamesDAO.Instance.GetCommentLisStringtById(Id));
            if (ListComment.Count > 0)
            {
                float total = 0f;
                foreach(var item in ListComment)
                {
                    total += (float)item.Rating;
                }
                AverageRating = total / (float)ListComment.Count;
            }

            DateModified = Convert.ToDateTime(row["dateModified"]);
        }


    }
}