using System.Collections.Generic;

namespace WebShop.ViewModels.Order
{
    public class GetOrdersResponse
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public bool HasNext => CurrentPage < TotalPages;

        public bool HasPrevious => CurrentPage > 1;

        public int PageSize => Orders.Count;

        public List<GetOrderResponse> Orders { get; set; } = new List<GetOrderResponse>();

    }
}
