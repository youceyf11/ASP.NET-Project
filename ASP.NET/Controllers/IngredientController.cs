using ASP.NET.Models;
using Microsoft.AspNetCore.Mvc;
using YourNamespace.Data;

namespace ASP.NET.Controllers;

public class IngredientController : Controller
{
    private Repository<Ingredient> ingredients;

    public IngredientController(YoussefDbContext context)
    {
        ingredients = new Repository<Ingredient>(context);
    }
    // GET
    public async Task<IActionResult> Index()
    {
        return View(await ingredients.GetAllAsync());
    }
    
    public async Task<IActionResult> Details(int id)
    {
        return View(await ingredients.GetByIdAsync(id, new QueryOptions<Ingredient>() {Includes = "ProductIngredients.Product"}));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IngredientId , Name")] Ingredient ingredient)
    {
        if (ModelState.IsValid)
        {
            await ingredients.AddAsync(ingredient);
            return RedirectToAction("Index");
        }

        return View(ingredient);
    }
}