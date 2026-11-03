using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.DTOs;
using ProductManagement.API.Models;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Services;

public class ProductService(AppDbContext dbContext) : IProductService
{
    public async Task<List<ProductDto>> GetAllAsync(int? categoryId)
    {
        var products = dbContext.Products.AsNoTracking();

        if (categoryId.HasValue)
        {
            products = products.Where(product => product.CategoryId == categoryId.Value);
        }

        return await products
            .OrderBy(product => product.Name)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CreatedOn = product.CreatedOn,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CreatedOn = product.CreatedOn,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto?> CreateAsync(CreateProductDto request)
    {
        var categoryExists = await dbContext.Categories
            .AnyAsync(category => category.Id == request.CategoryId);

        if (!categoryExists)
        {
            return null;
        }

        var product = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            Stock = request.Stock,
            CreatedOn = DateTime.UtcNow,
            CategoryId = request.CategoryId
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        return await GetByIdAsync(product.Id);
    }

    public async Task<ProductUpdateResult> UpdateAsync(int id, UpdateProductDto request)
    {
        var product = await dbContext.Products.FindAsync(id);

        if (product is null)
        {
            return ProductUpdateResult.NotFound;
        }

        var categoryExists = await dbContext.Categories
            .AnyAsync(category => category.Id == request.CategoryId);

        if (!categoryExists)
        {
            return ProductUpdateResult.CategoryNotFound;
        }

        product.Name = request.Name.Trim();
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;

        await dbContext.SaveChangesAsync();

        return ProductUpdateResult.Updated;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await dbContext.Products.FindAsync(id);

        if (product is null)
        {
            return false;
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();

        return true;
    }
}
