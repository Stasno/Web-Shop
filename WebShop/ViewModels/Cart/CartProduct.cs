using System;
using System.IO;

namespace WebShop.ViewModels.Cart
{
    public class CartProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int TotalPrice => Quantity * Price;
        public bool InStock { get; set; }
        public string Imghref
        {
            get
            {
                if (!File.Exists(Environment.CurrentDirectory + "/ClientApp/public/img/catalog/flower_" + ProductId + ".png"))
                {
                    return "/img/catalog/No image placeholder.png";
                }
                return "/img/catalog/flower_" + ProductId + ".png";
            }
        }
    }
}
