namespace ASP.NET.Models;

public class Order
{
    public int OrderId { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public String? UserId { get; set; }
    
    public User User { get; set; }
    
    public Product? Product { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; }
}