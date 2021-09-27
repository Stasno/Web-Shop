using Database;
using Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebShop.Services.Interfaces;
using WebShop.ViewModels.Cart;

namespace WebShop.Services
{
    public class CartServiceException : Exception
    {

        public CartServiceException(string message)
            : base(message)
        {
        }

        public CartServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
    public class CartService : ICartService
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _manager;
        public CartService(ApplicationContext context, UserManager<User> manager)
        {
            _context = context;
            _manager = manager;
        }

        public async Task AddItem(int productId, ClaimsPrincipal user)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new CartServiceException("This product doesnot exists");
            }

            var cart = await GetUserCartEntity(await _manager.GetUserAsync(user));

            CartItem item = await _context.CartItems.
                Where(c => c.Product == product && c.Cart == cart).FirstOrDefaultAsync();

            if (item == null)
            {
                _context.CartItems
                    .Add(new CartItem()
                    {
                        Cart = cart,
                        Product = product,
                        Quantity = 1
                    });
            }
            else
            {
                item.Quantity += 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task EmptyTheCart(ClaimsPrincipal user)
        {
            var cart = await GetUserCartEntity(await _manager.GetUserAsync(user));

            var items = await _context.CartItems.Where(c => c.Cart == cart).ToArrayAsync();

            if (items != null)
            {
                _context.CartItems.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<GetCartResponse> GetCart(ClaimsPrincipal user)
        {
            var cart = await GetUserCartEntity(await _manager.GetUserAsync(user));

            GetCartResponse result = new();

            result.Items = await _context.CartItems
                .Where(c => c.Cart == cart)
                .Include(c => c.Product)
                .Select(c => new CartProduct()
                {
                    Id = c.Id,
                    ProductId = c.Product.Id,
                    Title = c.Product.Title,
                    Quantity = c.Quantity,
                    Price = c.Product.Price,
                    InStock = c.Product.InStock >= c.Quantity,
                })
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task RemoveItem(int ItemId, ClaimsPrincipal user)
        {
            CartItem item = await _context.CartItems.FindAsync(ItemId);

            if (item == null)
                return;

            if (item.CartId == (await GetUserCartEntity(await _manager.GetUserAsync(user))).Id)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UpdateCartItemResponse> UpdateItem(int ItemId, int newQuantity, ClaimsPrincipal user)
        {
            CartItem item = await _context.CartItems
                .Where(i => i.Id == ItemId)
                .Include(i => i.Product)
                .FirstOrDefaultAsync();

            UpdateCartItemResponse result = new();
            result.InStock = false;

            if (item == null)
                return result;



            if (item.CartId == (await GetUserCartEntity(await _manager.GetUserAsync(user))).Id)
            {
                item.Quantity = newQuantity;

                result.InStock = item.Product.InStock >= newQuantity;

                await _context.SaveChangesAsync();

                return result;
            }

            return result;
        }

        public async Task<Cart> GetUserCartEntity(ClaimsPrincipal user)
        {
            return await GetUserCartEntity(await _manager.GetUserAsync(user));
        }

        public async Task<Cart> GetUserCartEntity(User user)
        {
            var cart = await _context.Carts.Where(c => c.User == user).FirstOrDefaultAsync();
            if (cart == null)
            {
                cart = new Cart() { User = user };
                cart = _context.Carts.Add(cart).Entity;
                await _context.SaveChangesAsync();
            }
            return cart;
        }
    }
}
