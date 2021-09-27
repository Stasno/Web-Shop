using System.Collections.Generic;

namespace WebShop.ViewModels.Order
{

    public class GetOrderResponse
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public int TotalPrice { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string PlacedAt { get; set; }
        public string LastUpdate { get; set; }

        public List<OrderProduct> Items { get; set; } = new List<OrderProduct>();

    }
}
