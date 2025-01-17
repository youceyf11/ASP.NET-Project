using ASP.NET.Models;
using Microsoft.AspNetCore.Mvc;
using YourNamespace.Data;

namespace ASP.NET.Controllers;

public class ProductController : Controller
{
    private Repository<Product> products;
    private Repository<Ingredient> ingredients;
    private Repository<Category> categories;
    private readonly IWebHostEnvironment _webHostEnvironement;

    public ProductController(YoussefDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        products = new Repository<Product>(context);
        ingredients = new Repository<Ingredient>(context);
        categories = new Repository<Category>(context);
        _webHostEnvironement = webHostEnvironment;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        return View(await products.GetAllAsync());
    }

    [HttpGet]
    public async Task<IActionResult> AddEdit(int id)
    {
        ViewBag.Ingredients = await ingredients.GetAllAsync();
        ViewBag.Categories = await categories.GetAllAsync();
        if (id == 0)
        {
            ViewBag.Operation = "Add";
            return View(new Product());
        }
        else
        {
            Product product = await products.GetByIdAsync(id, new QueryOptions<Product>
            {
                Includes = "ProductIngredients.Ingredient, Category",
             });
            ViewBag.Operation = "Edit";
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddEdit(Product product, int[] ingredientIds, int catId)
{
    if (ModelState.IsValid)
    {
        if (product.ImageFile != null)
        {
            string uploadFolder = Path.Combine(_webHostEnvironement.WebRootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
            }

            product.ImageUrl = uniqueFileName;
        }

        if (product.ProductId == 0)
        {
            ViewBag.Ingredients = await ingredients.GetAllAsync();
            ViewBag.Categories = await categories.GetAllAsync();
            product.CategoryId = catId;

            foreach (int id in ingredientIds)
            {
                product.ProductIngredients?.Add(new ProductIngredient
                    { IngredientId = id, ProductId = product.ProductId });
            }

            await products.AddAsync(product);
            return RedirectToAction("Index", "Product");
        }
        else
        {
            var existingProduct = await products.GetByIdAsync(product.ProductId, new QueryOptions<Product> { Includes = "ProductIngredients" });
            if (existingProduct == null)
            {
                ModelState.AddModelError("", "Product not found");
                ViewBag.Ingredients = await ingredients.GetAllAsync();
                ViewBag.Categories = await categories.GetAllAsync();
                return View(product);
            } 

            existingProduct.ProductIngredients?.Clear();
            foreach (int id in ingredientIds)
            {
                existingProduct.ProductIngredients?.Add(new ProductIngredient
                    { IngredientId = id, ProductId = product.ProductId });
            }

            try
            {
                await products.UpdateAsync(existingProduct);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Error: {e.GetBaseException().Message}");
                ViewBag.Ingredients = await ingredients.GetAllAsync();
                ViewBag.Categories = await categories.GetAllAsync();
                return View(product);
            }
        }
    }

    return RedirectToAction("Index", "Product");
}
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
            try
            {
                await products.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch 
            {
                ModelState.AddModelError("", $"Product not found.");
                return RedirectToAction("Index", "Product");
            }
        
    }
}
