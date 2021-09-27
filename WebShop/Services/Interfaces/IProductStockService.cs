using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebShop.Services.Interfaces
{
    public interface IProductStockService
    {
        public bool IsInStock(List<CartItem> cartItems, out List<int> ProductsOutOfStock);
        public bool IsInStock(Product product, int quantity);
        public Task ReturnProductsToStock(List<OrderItem> orderItems);
        public Task GetProductsFromStock(List<CartItem> cartItems);
        public bool UpdateQuantity(Product product, int newQuantity);
        public bool DecQuantity(Product product, int count);
        public bool IncQuantity(Product product, int count);

    }
}
