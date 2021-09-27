using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebShop.Services.Interfaces;
using WebShop.ViewModels.Catalog;

namespace WebShop.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalog;
        public CatalogController(ICatalogService catalogService)
        {
            _catalog = catalogService;
        }

        /// <summary>
        /// Returns product page
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductPageRequest request)
        {

            if (!string.IsNullOrEmpty(request.Category))
            {
                _catalog.SelectCategory(request.Category);
            }
            if (!string.IsNullOrEmpty(request.SearchName))
            {
                _catalog.SearchByName(request.SearchName);
            }
            if (!string.IsNullOrEmpty(request.Sort))
            {
                _catalog.SortBy(request.Sort);
            }

            var result = await _catalog.ExecuteQueryAndGetPage(request.Index, request.Limit);

            var metadata = new
            {
                result.CurrentPage,
                result.PageSize,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(result);
        }


        /// <summary>
        /// Returns product by specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _catalog.GetProduct(id);
            if (result == null)
            {
                return NotFound(id);
            }
            return Ok(result);
        }

        /// <summary>
        /// Returns product names and codes that match the name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string name,
            [Range(1, 51)]
            [FromQuery] int limit = 5)
        {

            if (!string.IsNullOrEmpty(name))
            {
                _catalog.SearchByName(name);
            }

            var result = await _catalog.ExecuteQueryAndGetSearchResult(limit);

            return Ok(result);
        }

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost("product")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
        {
            await _catalog.AddProduct(request);
            return Ok();
        }

        /// <summary>
        /// Updates the product in the database
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPut("product")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            await _catalog.UpdateProduct(request);
            return Ok();
        }
    }
}
