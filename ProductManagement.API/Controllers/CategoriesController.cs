using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.DTOs;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        var categories = await categoryService.GetAllAsync();
        return Ok(categories);
    }
}
