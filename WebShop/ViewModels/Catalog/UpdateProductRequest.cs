using System;
using System.ComponentModel.DataAnnotations;

namespace WebShop.ViewModels.Catalog
{
    public class UpdateProductRequest : AddProductRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
    }
}
