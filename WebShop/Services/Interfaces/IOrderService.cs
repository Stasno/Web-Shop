using Database.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebShop.ViewModels.Order;

namespace WebShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task<GetOrdersResponse> GetOrders(ClaimsPrincipal user, int limit, int offset);
        Task<GetOrderResponse> GetOrder(int id, ClaimsPrincipal user);

        Task CreateOrder(AddOrderRequest newOrder, ClaimsPrincipal user);
        Task CreateOrder(AddOrderRequest newOrder, List<CartItem> cartItems, User user)
;
        Task CancelOrderAndReturnProduct(int orderId, string Description, ClaimsPrincipal user);
        Task SetCancelledState(Order order, string Description);

        Task SetOrderState(int orderId, string newState, string Description, ClaimsPrincipal user);
        Task SetOrderState(Order order, string newState, string Description);
    }
}
