using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebShop.Services;
using WebShop.Services.Interfaces;
using WebShop.ViewModels;

namespace WebShop.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cart;
        public CartController(ICartService CartService)
        {
            _cart = CartService;
        }

        /// <summary>
        /// Returns all items in the cart of the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetCart()
        {
            var result = await _cart.GetCart(User);
            return Ok(result);
        }

        /// <summary>
        /// Clears the user's cart
        /// </summary>
        /// <returns></returns>
        [HttpDelete("")]
        public async Task<IActionResult> EmptyTheCart()
        {
            await _cart.EmptyTheCart(User);
            return Ok();
        }

        /// <summary>
        /// Adds a product to the cart or, if it exists, increases the quantity
        /// </summary>
        /// <param name="Productid"></param>
        /// <returns></returns>
        [HttpPost("{Productid}")]
        public async Task<IActionResult> AddItem(int Productid)
        {
            try
            {
                await _cart.AddItem(Productid, User);
            }
            catch (CartServiceException e)
            {
                return BadRequest(new ErrorResponse<int>(400, e.Message, Productid));
            }
            return Ok();
        }

        /// <summary>
        /// Sets the new quantity of items in the cart 
        /// </summary>
        /// <param name="Itemid"></param>
        /// <param name="newQuantity"></param>
        /// <returns></returns>
        [HttpPut("{Itemid}")]
        public async Task<IActionResult> UpdateItem(int Itemid,
            [Required]
            [Range(1, int.MaxValue)]
            [FromBody] int newQuantity)
        {
            var result = await _cart.UpdateItem(Itemid, newQuantity, User);
            return Ok(result);
        }


        /// <summary>
        /// Removes an item from the cart
        /// </summary>
        /// <param name="Itemid"></param>
        /// <returns></returns>
        [HttpDelete("{Itemid}")]
        public async Task<IActionResult> RemoveItem(int Itemid)
        {
            await _cart.RemoveItem(Itemid, User);
            return Ok();
        }
    }
}
