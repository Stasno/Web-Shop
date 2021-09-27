using System.Threading.Tasks;
using WebShop.ViewModels.Catalog;

namespace WebShop.Services.Interfaces
{
    public interface ICatalogService
    {
        void SearchByName(string search);

        void SelectCategory(string tag);

        void SortBy(string sort);

        Task<ProductPage> ExecuteQueryAndGetPage(int index, int limit);

        Task<SearchResponse> ExecuteQueryAndGetSearchResult(int limit);

        Task<GetProductResponse> GetProduct(int productId);

        Task AddProduct(AddProductRequest newProduct);

        Task UpdateProduct(UpdateProductRequest newProduct);

    }
}
