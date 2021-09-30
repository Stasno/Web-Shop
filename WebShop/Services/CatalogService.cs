using Database;
using Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Services.Interfaces;
using WebShop.ViewModels.Catalog;

namespace WebShop.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        private IQueryable<Product> _productsQuery;
        public CatalogService(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _productsQuery = context.Products;
        }

        public async Task<ProductPage> ExecuteQueryAndGetPage(int index, int limit)
        {
            ProductPage page = new();

            page.TotalPages =
                Convert.ToInt32(
                    Math.Ceiling(
                        await _productsQuery.CountAsync() / Convert.ToDouble(limit)));

            _productsQuery = _productsQuery.Skip((index - 1) * limit).Take(limit);

            page.Products = await _productsQuery.
                Select(p => new CatalogProduct()
                {
                    Id = p.Id,
                    Price = p.Price,
                    Title = p.Title,
                    InStock = p.InStock > 0
                })
                .AsNoTracking()
                .ToListAsync();

            page.CurrentPage = index;

            return page;
        }

        public void SearchByName(string search)
        {
            _productsQuery = _productsQuery.Where(p => p.Title.Contains(search));
        }

        public void SelectCategory(string tag)
        {
            _productsQuery = _productsQuery.Where(p => p.Categories.Any(t => t.Name == tag));
        }

        public void SortBy(string sort)
        {
            switch (sort)
            {
                case "name_desc":
                    _productsQuery = _productsQuery.OrderByDescending(p => p.Title);
                    break;
                case "name_asc":
                    _productsQuery = _productsQuery.OrderBy(p => p.Title);
                    break;
                case "price_desc":
                    _productsQuery = _productsQuery.OrderByDescending(p => p.Price);
                    break;
                case "price_asc":
                    _productsQuery = _productsQuery.OrderBy(p => p.Price);
                    break;
            }

        }

        public async Task<SearchResponse> ExecuteQueryAndGetSearchResult(int limit)
        {
            SearchResponse result = new();

            result.Count = await _productsQuery.CountAsync();

            result.Items = await _productsQuery.Take(limit)
                                         .Select(p => new SearchItem()
                                         {
                                             Id = p.Id,
                                             Title = p.Title
                                         })
                                         .AsNoTracking()
                                         .ToListAsync();

            return result;
        }

        public async Task<GetProductResponse> GetProduct(int productId)
        {
            GetProductResponse result;
            result = await _context.Products
                .Include(c => c.Categories)
                .Select(p => new GetProductResponse(p))
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task AddProduct(AddProductRequest newProduct)
        {
            Product product = newProduct.CreateEntity();
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            if (newProduct.Image != null)
            {
                string path = "/ClientApp/public/img/catalog/flower_" + product.Id + ".png";

                using (var fileStream = new FileStream(_appEnvironment.ContentRootPath + path, FileMode.Create))
                {
                    await newProduct.Image.CopyToAsync(fileStream);
                }
            }


        }

        public async Task UpdateProduct(UpdateProductRequest newProduct)
        {
            Product product = await _context.Products.FindAsync(newProduct.Id);

            if (product == null)
            {
                return;
            }

            product.Title = newProduct.Title;
            product.Price = newProduct.Price;
            product.InStock = newProduct.InStock;
            product.Description = newProduct.Description;
            if (newProduct.Categories != null)
            {
                foreach (var i in newProduct.Categories)
                {
                    product.Categories.Add(new Category()
                    {
                        Id = i.Id,
                        Name = i.Name
                    });
                }
            }

            await _context.SaveChangesAsync();

            if (newProduct.Image != null)
            {
                string path = "/ClientApp/public/img/catalog/flower_" + product.Id + ".png";

                using (var fileStream = new FileStream(_appEnvironment.ContentRootPath + path, FileMode.Create))
                {
                    await newProduct.Image.CopyToAsync(fileStream);
                }
            }

        }
    }
}
