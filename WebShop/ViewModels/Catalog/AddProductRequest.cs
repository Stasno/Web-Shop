using Database.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.ViewModels.Catalog
{
    public class AddProductRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int InStock { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public List<CategoryViewModel> Categories { get; set; }

        public Product CreateEntity()
        {
            Product product = new()
            {
                Title = this.Title,
                Price = this.Price,
                InStock = this.InStock,
                Description = this.Description,
            };

            foreach (var i in Categories)
            {
                product.Categories.Add(new Category()
                {
                    Id = i.Id,
                    Name = i.Name,
                });
            }

            return product;
        }

    }
}

