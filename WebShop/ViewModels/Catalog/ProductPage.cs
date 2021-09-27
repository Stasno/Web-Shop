using System.Collections.Generic;

namespace WebShop.ViewModels.Catalog
{
    public class ProductPage
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public bool HasNext => CurrentPage < TotalPages;

        public bool HasPrevious => CurrentPage > 1;

        public int PageSize => Products.Count;

        public List<CatalogProduct> Products { get; set; }

    }
}
