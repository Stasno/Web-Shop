using System.ComponentModel.DataAnnotations;

namespace WebShop.ViewModels.Catalog
{
    public class ProductPageRequest
    {
        public string Category { get; set; }
        public string SearchName { get; set; }
        public string Sort { get; set; }
        [Range(1, 50)]
        public int Limit { get; set; } = 12;
        [Range(1, int.MaxValue)]
        public int Index { get; set; } = 1;
    }
}
