using System;
using System.IO;

namespace WebShop.ViewModels.Catalog
{
    public class CatalogProduct
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public bool InStock { get; set; }
        public string Imghref
        {
            get
            {
                if (!File.Exists(Environment.CurrentDirectory + "/ClientApp/public/img/catalog/flower_" + Id + ".png"))
                {
                    return "/img/catalog/No image placeholder.png";
                }
                return "/img/catalog/flower_" + Id + ".png";
            }
        }
    }
}
