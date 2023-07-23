using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO.GameModel
{
    public class GamesInCart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgName { get; set; }

        public int Quantity { get; set; }
        public float PriceGame { get; set; }
        
        public GamesInCart()
        {
            // Không làm gì cả, để các thuộc tính giữ giá trị mặc định của kiểu dữ liệu
        }

        // Constructor với tham số (parameterized constructor)
        public GamesInCart(int id, string name, string imgName,int quantity, float priceGame)
        {
            Id = id;
            Name = name;
            ImgName = imgName;
            Quantity = quantity;
            PriceGame = priceGame;
        }

    }
}