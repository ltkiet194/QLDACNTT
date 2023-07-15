using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO.GameModel
{
    public class Comment
    {
        public int Id { get; set; }
        public int IdKhachHang { get; set; }

        public string TenKH { get; set; }
        // public string Avatar { get; set; }
        public string Content { get; set; }
        public int  Rating { get; set; }
        public DateTime DateModified { get; set; }

        public Comment(int id, int idKhachHang, string content,int rating,string tenKh)
        {
            Id = id;
            IdKhachHang = idKhachHang;
            TenKH = tenKh;
            //Avatar = avatar;
            Content = content;
            Rating = rating;
            DateModified = DateTime.Now;
        }
    }
}