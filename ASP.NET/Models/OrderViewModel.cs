namespace ASP.NET.Models;

public class OrderViewModel
{
    public decimal TotalAmount { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; }
    public List<Product> Products { get; set; }
}