using GameStore.Models.DAO;
using GameStore.Models.DTO.GameModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO
{
    public class UserInfo
    {
        public int Id { get; set; }
        public KHACHHANG KhachHang { get; set; }

        public List<TBAddFriend> SLBanBeGuiLoiMoi { get; set; }
        

        public List<GamesInCart> listGames { get; set; }

        public UserInfo()
        {

        }
        public UserInfo(KHACHHANG kh)
        {
            Id = kh.MaKH;
            KhachHang = kh;
            listGames= JsonConvert.DeserializeObject<List<GamesInCart>>(GamesDAO.Instance.GetGameCartByIdKH(kh.MaKH));
            SLBanBeGuiLoiMoi = GamesDAO.Instance.GetSLBanbechuakb(kh);
        }


    }
}