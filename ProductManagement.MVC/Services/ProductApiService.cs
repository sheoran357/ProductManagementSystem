using System.Net;
using System.Net.Http.Json;
using ProductManagement.MVC.Models;

namespace ProductManagement.MVC.Services;

public class ProductApiService(HttpClient httpClient) : IProductApiService
{
    public async Task<List<ProductViewModel>> GetProductsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ProductViewModel>>("api/products")
            ?? [];
    }

    public async Task<ProductViewModel?> GetProductAsync(int id)
    {
        var response = await httpClient.GetAsync($"api/products/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProductViewModel>();
    }

    public async Task<List<CategoryViewModel>> GetCategoriesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<CategoryViewModel>>("api/categories")
            ?? [];
    }

    public Task<HttpResponseMessage> CreateProductAsync(ProductFormViewModel product)
    {
        var request = new
        {
            product.Name,
            product.Price,
            product.Stock,
            product.CategoryId
        };

        return httpClient.PostAsJsonAsync("api/products", request);
    }

    public Task<HttpResponseMessage> UpdateProductAsync(int id, ProductFormViewModel product)
    {
        var request = new
        {
            product.Name,
            product.Price,
            product.Stock,
            product.CategoryId
        };

        return httpClient.PutAsJsonAsync($"api/products/{id}", request);
    }

    public Task<HttpResponseMessage> DeleteProductAsync(int id)
    {
        return httpClient.DeleteAsync($"api/products/{id}");
    }
}
