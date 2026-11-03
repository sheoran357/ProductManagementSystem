using System.Net.Http.Json;
using ProductManagement.MVC.Models;

namespace ProductManagement.MVC.Services;

public class ProductApiService(HttpClient httpClient) : IProductApiService
{
    public async Task<List<ProductViewModel>> GetProductsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ProductViewModel>>("api/products")
            ?? new List<ProductViewModel>();
    }

    public Task<ProductViewModel?> GetProductAsync(int id)
    {
        return httpClient.GetFromJsonAsync<ProductViewModel>($"api/products/{id}");
    }

    public async Task<List<CategoryViewModel>> GetCategoriesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<CategoryViewModel>>("api/categories")
            ?? new List<CategoryViewModel>();
    }

    public Task<HttpResponseMessage> CreateProductAsync(ProductFormViewModel product)
    {
        return httpClient.PostAsJsonAsync("api/products", product);
    }

    public Task<HttpResponseMessage> UpdateProductAsync(int id, ProductFormViewModel product)
    {
        return httpClient.PutAsJsonAsync($"api/products/{id}", product);
    }

    public Task<HttpResponseMessage> DeleteProductAsync(int id)
    {
        return httpClient.DeleteAsync($"api/products/{id}");
    }
}
