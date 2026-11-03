using ProductManagement.API.DTOs;

namespace ProductManagement.API.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();

    Task<bool> ExistsAsync(int id);
}
