using ProductManagement.MVC.Models;

namespace ProductManagement.MVC.Services;

public interface IProductApiService
{
    Task<List<ProductViewModel>> GetProductsAsync();

    Task<ProductViewModel?> GetProductAsync(int id);

    Task<List<CategoryViewModel>> GetCategoriesAsync();

    Task<HttpResponseMessage> CreateProductAsync(ProductFormViewModel product);

    Task<HttpResponseMessage> UpdateProductAsync(int id, ProductFormViewModel product);

    Task<HttpResponseMessage> DeleteProductAsync(int id);
}
