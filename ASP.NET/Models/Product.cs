using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ASP.NET.Models;

public class Product
{
    public int ProductId { get; set; }
    public String? Name { get; set; }
    public String? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    
    public string ImageUrl { get; set; }= "default.jpg";
    [ValidateNever]
    public Category? Category { get; set; }
    
    [ValidateNever]
    public ICollection<OrderItem>? OrderItems { get; set; }
    
    [ValidateNever]
    public ICollection<ProductIngredient>? ProductIngredients { get; set; }
}