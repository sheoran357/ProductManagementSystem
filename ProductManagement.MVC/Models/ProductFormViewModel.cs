using System.ComponentModel.DataAnnotations;

namespace ProductManagement.MVC.Models;

public class ProductFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Product name")]
    public string Name { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public List<CategoryViewModel> Categories { get; set; } = [];
}
