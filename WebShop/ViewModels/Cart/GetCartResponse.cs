using System.Collections.Generic;

namespace WebShop.ViewModels.Cart
{
    public class GetCartResponse
    {
        public int Size => Items.Count;
        public List<CartProduct> Items { get; set; }

    }
}
