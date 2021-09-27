using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebShop.Services;
using WebShop.Services.Interfaces;
using WebShop.ViewModels;
using WebShop.ViewModels.Order;

namespace WebShop.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _order;
        public OrderController(IOrderService order)
        {
            _order = order;
        }

        /// <summary>
        /// Returns all orders by current authrorized user 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetOrders(
            [Range(1, 50)]
            int limit = 15,
            [Range(1, int.MaxValue)]
            int index = 1)
        {
            GetOrdersResponse result = await _order.GetOrders(User, index, limit);
            return Ok(result);
        }

        /// <summary>
        /// Returns order by specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            GetOrderResponse result;
            try
            {
                result = await _order.GetOrder(id, User);
            }
            catch (OrderServiceException ex)
            {
                return BadRequest(new ErrorResponse<int>(400, ex.Message, id));
            }

            if (result == null)
            {
                return NotFound(id);
            }

            return Ok(result);

        }

        /// <summary>
        /// Creates an order and receives an item from the cart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> CreateOrder([FromBody] AddOrderRequest request)
        {
            try
            {
                await _order.CreateOrder(request, User);
            }
            catch (OutOfStockException ex)
            {
                return BadRequest(new ErrorResponse<int>(400, ex.Message, ex.ProductsOutOfStock));
            }
            catch (OrderServiceException ex)
            {
                return BadRequest(new ErrorResponse<AddOrderRequest>(400, ex.Message, request));
            }
            return Ok();
        }


        /// <summary>
        /// Sets an specific order to "Cancelled" state
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id,
            [MaxLength(2048)]
            string description)
        {
            try
            {
                await _order.CancelOrderAndReturnProduct(id, description, User);
            }
            catch (OrderServiceException ex)
            {
                return BadRequest(new ErrorResponse<int>(400, ex.Message, id));
            }

            return Ok();
        }

        /// <summary>
        /// Changes order status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newState"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderState(int id,
            [Required]
            [MaxLength(64)]
            string newState,
            [MaxLength(2048)]
            string description)
        {
            try
            {
                await _order.SetOrderState(id, newState, description, User);
            }
            catch (OrderServiceException ex)
            {
                return BadRequest(
                    new ErrorResponse<dynamic>(400, ex.Message, new { id, newState }));
            }
            return Ok();
        }

    }
}
