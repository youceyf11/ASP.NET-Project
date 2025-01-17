using ASP.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using YourNamespace.Data;

namespace ASP.NET.Controllers;

public class OrderController : Controller
{
    private readonly YoussefDbContext _context;

    private readonly Repository<Product> _products;
    private readonly Repository<Order> _orders;

    private readonly UserManager<User> _userManager;

    public OrderController(YoussefDbContext context, Repository<Product> products, Repository<Order> orders,
        UserManager<User> userManager)
    {
        _context = context;
        _products = products;
        _orders = orders;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = HttpContext.Session.Get<OrderViewModel>("OrderViewModel") ?? new OrderViewModel(
            OrderItems: new List<OrderItemViewModel>(),
            Products = await _products.GetAllAsync()
    };
    
            return View(model);
        }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddItem(int prodId, int prodQty)
    {
        var product = await _context.Products.FindAsync(prodId);
        if (product == null)
        {
            return NotFound();
        }
        var model=HttpContext.Session.Get<OrderViewModel>("OrderViewModel")??new OrderViewModel();
        var orderItem = new OrderItemViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = prodQty
        };
        model.OrderItems.Add(orderItem);
        HttpContext.Session.Set("OrderViewModel", model);
        return RedirectToAction("Create");
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
}