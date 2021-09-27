using Database.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using WebShop.ViewModels.Cart;

namespace WebShop.Services.Interfaces
{
    public interface ICartService
    {
        Task AddItem(int productId, ClaimsPrincipal user);
        Task RemoveItem(int ItemId, ClaimsPrincipal user);
        Task<UpdateCartItemResponse> UpdateItem(int ItemId, int newQuantity, ClaimsPrincipal user);
        Task<GetCartResponse> GetCart(ClaimsPrincipal user);
        Task EmptyTheCart(ClaimsPrincipal user);
        Task<Cart> GetUserCartEntity(ClaimsPrincipal user);
        Task<Cart> GetUserCartEntity(User user);

    }
}
