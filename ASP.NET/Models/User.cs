using Microsoft.AspNetCore.Identity;

namespace ASP.NET.Models;

public class User : IdentityUser
{
    public ICollection<Order>? Orders { get; set; }
}