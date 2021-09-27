using Database;
using Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebShop.Services.Interfaces;
using WebShop.ViewModels.Order;

namespace WebShop.Services
{

    public class OrderServiceException : Exception
    {

        public OrderServiceException(string message)
            : base(message)
        {
        }

        public OrderServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }

    public class OrderService : IOrderService
    {
        private readonly ApplicationContext _context;
        private readonly IProductStockService _productStock;
        private readonly UserManager<User> _manager;
        public OrderService(ApplicationContext context,
            IProductStockService productStock,
            UserManager<User> manager)
        {
            _context = context;
            _productStock = productStock;
            _manager = manager;
        }

        public async Task<GetOrderResponse> GetOrder(int id, ClaimsPrincipal user)
        {
            var orderUserId = await _context.Orders
                .Where(o => o.Id == id)
                .Select(o => o.UserId)
                .FirstOrDefaultAsync();

            if (orderUserId != _manager.GetUserId(user))
            {
                throw new OrderServiceException("Wrong user");
            }

            var order = await _context.Orders
                .Where(o => o.Id == id)

                .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Product)

                .Include(o => o.OrderOrderStates
                        .OrderByDescending(i => i.UpdatedAt))
                    .ThenInclude(o => o.OrderState)

                .Select(o => new GetOrderResponse()
                {
                    Id = o.Id,

                    OrderStatus = (o.OrderOrderStates
                                        .FirstOrDefault() == null
                                        ?
                                        "Placed" //true
                                        :
                                        o.OrderOrderStates //false
                                            .FirstOrDefault()
                                            .OrderState
                                            .Name),

                    LastUpdate = (o.OrderOrderStates
                                        .FirstOrDefault() == null
                                        ?
                                        o.PlacedAt.ToShortDateString() // true
                                        :
                                        o.OrderOrderStates //false
                                            .FirstOrDefault()
                                            .UpdatedAt.ToShortDateString()),

                    TotalPrice = o.TotalPrice,
                    ZipCode = o.ZipCode,
                    Address = o.Address,
                    City = o.City,
                    Country = o.Country,
                    PlacedAt = o.PlacedAt.ToShortDateString(),
                    Items = o.OrderItems.Select(i => new OrderProduct()
                    {
                        Id = i.Product.Id,
                        Title = i.Product.Title,
                        Price = i.Product.Price,
                        Quantity = i.Quantity
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return order;

        }

        public async Task<GetOrdersResponse> GetOrders(ClaimsPrincipal user, int index, int limit)
        {

            var userId = _manager.GetUserId(user);

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(i => i.PlacedAt)

                .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Product)

                .Include(o => o.OrderOrderStates
                        .OrderByDescending(i => i.UpdatedAt))
                    .ThenInclude(o => o.OrderState)

                .Select(o => new GetOrderResponse()
                {
                    Id = o.Id,

                    OrderStatus = (o.OrderOrderStates
                                        .FirstOrDefault() == null
                                        ?
                                        "Placed" //true
                                        :
                                        o.OrderOrderStates //false
                                            .FirstOrDefault()
                                            .OrderState
                                            .Name),

                    LastUpdate = (o.OrderOrderStates
                                        .FirstOrDefault() == null
                                        ?
                                        o.PlacedAt.ToShortDateString() // true
                                        :
                                        o.OrderOrderStates //false
                                            .FirstOrDefault()
                                            .UpdatedAt.ToShortDateString()),

                    TotalPrice = o.TotalPrice,
                    ZipCode = o.ZipCode,
                    Address = o.Address,
                    City = o.City,
                    Country = o.Country,
                    PlacedAt = o.PlacedAt.ToShortDateString(),
                    Items = o.OrderItems.Select(i => new OrderProduct()
                    {
                        Id = i.Product.Id,
                        Title = i.Product.Title,
                        Price = i.Product.Price,
                        Quantity = i.Quantity
                    }).ToList()
                })
                .Skip((index - 1) * limit)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();

            var result = new GetOrdersResponse
            {
                Orders = orders,
                CurrentPage = index,
            };

            int count = await _context.Orders.Where(o => o.UserId == userId).CountAsync();

            result.TotalPages =
                Convert.ToInt32(
                    Math.Ceiling(
                        count / Convert.ToDouble(limit)));

            return result;
        }

        public async Task CreateOrder(AddOrderRequest newOrder, ClaimsPrincipal user)
        {
            var userBuf = await _manager.GetUserAsync(user);

            var cartItems = await _context.Carts
                .Where(c => c.User == userBuf)
                .Include(c => c.CartItems)
                    .ThenInclude(c => c.Product)
                .Select(c => c.CartItems)
                .FirstOrDefaultAsync();

            await CreateOrder(newOrder, cartItems, userBuf);

        }
        public async Task CreateOrder(AddOrderRequest newOrder, List<CartItem> cartItems, User user)
        {

            if (cartItems == null)
            {
                throw new OrderServiceException("Your cart is empty");
            }

            if (cartItems.Count < 1)
            {
                throw new OrderServiceException("Your cart is empty");
            }

            Order order = new()
            {
                TotalPrice = 0,
                PhoneNumber = newOrder.PhoneNumber,
                ZipCode = newOrder.ZipCode,
                Address = newOrder.Address,
                City = newOrder.City,
                Country = newOrder.Country,
                PlacedAt = DateTime.Now,
                User = user
            };

            foreach (var i in cartItems)
            {
                order.TotalPrice += i.Product.Price * i.Quantity;
            }

            _context.Orders.Add(order);

            List<OrderItem> items = new(cartItems.Count);

            for (int i = 0; i < cartItems.Count; i++)
            {
                items.Add(new OrderItem());
                items[i].Product = cartItems[i].Product;
                items[i].Quantity = cartItems[i].Quantity;
            }

            order.OrderItems = items;

            await _context.SaveChangesAsync();

            try
            {
                await _productStock.GetProductsFromStock(cartItems);
            }
            catch (OutOfStockException)
            {
                await SetCancelledState(order, "Out of stock");
                throw;
            }

        }

        public Task SetCancelledState(Order order, string Description)
        {
            return SetOrderState(order, "Cancelled", Description);
        }
        public async Task CancelOrderAndReturnProduct(int orderId, string Description, ClaimsPrincipal user)
        {
            bool check = await _context.OrderOrderStates
                .Where(i => i.OrderId == orderId && i.OrderState.Name == "Cancelled")
                .Include(i => i.OrderState)
                .AnyAsync();

            if (check == true)
            {
                throw new OrderServiceException("order already canceled");
            }

            await SetOrderState(orderId, "Cancelled", Description, user);
            var items = await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .Include(i => i.Product)
                .ToListAsync();

            await _productStock.ReturnProductsToStock(items);
        }

        public async Task SetOrderState(int orderId,
            string newState,
            string Description,
            ClaimsPrincipal user)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                throw new OrderServiceException("This order does not exist");
            }

            if (order.UserId != _manager.GetUserId(user))
            {
                throw new OrderServiceException("Wrong user");
            }

            await SetOrderState(order, newState, Description);

        }

        public async Task SetOrderState(Order order,
            string newState,
            string Description)
        {

            OrderState state = await _context
                .OrderStates
                .Where(s => s.Name == newState)
                .FirstOrDefaultAsync();

            if (state == null)
            {
                throw new OrderServiceException("Unknown state: \"" + newState + "\"");
            }

            var checkState = await _context.OrderOrderStates
                .Where(i => i.Order == order)
                .OrderByDescending(i => i.UpdatedAt)
                .Select(i => i.OrderStateId)
                .FirstOrDefaultAsync();

            if (state.Id == checkState)
            {
                throw new OrderServiceException("The order is already in this state");
            }

            OrderOrderState orderState = new();

            orderState.Order = order;
            orderState.OrderState = state;
            orderState.UpdatedAt = DateTime.Now;
            orderState.Description = Description;

            _context.OrderOrderStates.Add(orderState);

            await _context.SaveChangesAsync();

        }
    }
}
