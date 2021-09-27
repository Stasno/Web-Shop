using Database.Models;

namespace WebShop.ViewModels.Catalog
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryViewModel()
        {

        }

        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }
    }
}
