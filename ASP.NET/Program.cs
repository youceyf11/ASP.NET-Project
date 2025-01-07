using ASP.NET.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<YoussefDbContext>(options =>
    options.UseNpgsql(connectionString));

// Use either AddIdentity or AddDefaultIdentity, but not both
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<YoussefDbContext>();

// builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddRoles<IdentityRole>() // Add this line to enable roles
//     .AddEntityFrameworkStores<YoussefDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Add this line to enable authentication
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();