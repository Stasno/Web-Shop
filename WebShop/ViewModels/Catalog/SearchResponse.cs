using System.Collections.Generic;

namespace WebShop.ViewModels.Catalog
{
    public class SearchItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class SearchResponse
    {
        public int Count { get; set; }

        public List<SearchItem> Items { get; set; }
    }
}
