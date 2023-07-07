using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.Models;
namespace GameStore.Models.DTO
{
    public class Users
    {
        public int MaKH { get; set; }
        public string HoTen { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public DateTime NgaySinh { get; set; }
        public string XacMinh { get; set; }
        public int TrangThai { get; set; }

    }
}