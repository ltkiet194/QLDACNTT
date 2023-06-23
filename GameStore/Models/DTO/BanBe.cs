using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;

namespace GameStore.Models.DTO
{
    public class BanBe
    {
        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "ID Bạn Bè")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "ID Khách hàng")]
        public int Id_KH { get; set; }

        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "ListFriends")]
        public List<Friend> ListFriends { get; set; }

        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "Ngày Sửa đổi")]
        public DateTime dateModifỉe { get; set; }

        public BanBe(DataRow row)
        {
            this.Id = (int)row["Id"];
            this.Id_KH = (int)row["Id_KH"];
            this.ListFriends = ListBanBe(row["ListFriends"].ToString());
            this.dateModifỉe = (DateTime)row["dateModifỉer"];
        }

        List<Friend> ListBanBe(string fr)
        {
            List<Friend> friends = JsonSerializer.Deserialize<List<Friend>>(fr);
            return friends ?? new List<Friend>();
        }
    }


    public class Friend
    {
        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "ID Bạn Bè")]
        public int Id_Banbe { get; set; }

        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "Trạng Thái")]
        public int TrangThai { get; set; }

        [Required(ErrorMessage = "Lỗi")]
        [Display(Name = "Ngày Gửi")]
        public DateTime NgayGui { get; set; }
    }
}
