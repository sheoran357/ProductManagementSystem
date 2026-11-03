using ProductManagement.API.DTOs;

namespace ProductManagement.API.Services.Interfaces;

public enum ProductUpdateResult
{
    Updated,
    NotFound,
    CategoryNotFound
}

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync(int? categoryId);

    Task<ProductDto?> GetByIdAsync(int id);

    Task<ProductDto?> CreateAsync(CreateProductDto request);

    Task<ProductUpdateResult> UpdateAsync(int id, UpdateProductDto request);

    Task<bool> DeleteAsync(int id);
}
