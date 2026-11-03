using Microsoft.AspNetCore.Mvc;
using ProductManagement.MVC.Models;
using ProductManagement.MVC.Services;

namespace ProductManagement.MVC.Controllers;

public class ProductsController(IProductApiService productApiService) : Controller
{
    public async Task<IActionResult> Index()
    {
        try
        {
            var products = await productApiService.GetProductsAsync();
            return View(products);
        }
        catch (HttpRequestException)
        {
            ViewBag.Error = "The API is not available. Start the API project and refresh this page.";
            return View(new List<ProductViewModel>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new ProductFormViewModel();
        await LoadCategoriesAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(model);
            return View(model);
        }

        using var response = await productApiService.CreateProductAsync(model);

        if (response.IsSuccessStatusCode)
        {
            TempData["Message"] = "Product created successfully.";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty,
            "The product could not be created. Check the values and selected category.");

        await LoadCategoriesAsync(model);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await productApiService.GetProductAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        var model = new ProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId
        };

        await LoadCategoriesAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductFormViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(model);
            return View(model);
        }

        using var response = await productApiService.UpdateProductAsync(id, model);

        if (response.IsSuccessStatusCode)
        {
            TempData["Message"] = "Product updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }

        ModelState.AddModelError(string.Empty,
            "The product could not be updated. Check the values and selected category.");

        await LoadCategoriesAsync(model);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await productApiService.GetProductAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        using var response = await productApiService.DeleteProductAsync(id);

        if (!response.IsSuccessStatusCode &&
            response.StatusCode != System.Net.HttpStatusCode.NotFound)
        {
            return BadRequest("The product could not be deleted.");
        }

        TempData["Message"] = "Product deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync(ProductFormViewModel model)
    {
        model.Categories = await productApiService.GetCategoriesAsync();
    }
}
