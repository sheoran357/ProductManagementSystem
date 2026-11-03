using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.DTOs;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Services;

public class CategoryService(AppDbContext dbContext) : ICategoryService
{
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await dbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            })
            .ToListAsync();
    }

    public Task<bool> ExistsAsync(int id)
    {
        return dbContext.Categories.AnyAsync(category => category.Id == id);
    }
}
