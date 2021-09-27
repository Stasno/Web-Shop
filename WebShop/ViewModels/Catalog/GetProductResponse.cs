using Database.Models;
using System.Collections.Generic;

namespace WebShop.ViewModels.Catalog
{


    public class GetProductResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public int InStock { get; set; }
        public string Description { get; set; }
        public List<CategoryViewModel> Categories { get; set; }

        public GetProductResponse()
        {
        }

        public GetProductResponse(Product product)
        {
            Id = product.Id;
            Title = product.Title;
            Price = product.Price;
            InStock = product.InStock;
            Description = product.Description;
            Categories = new(product.Categories.Count);

            foreach (var i in product.Categories)
            {
                Categories.Add(new CategoryViewModel(i));
            }

        }

    }
}
