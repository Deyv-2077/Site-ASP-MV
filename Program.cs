using Microsoft.EntityFrameworkCore;
using MVCApp2.Models;
using MVCApp2.Repositorio;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using MVCApp2.Helper;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddScoped<ISessao, Sessao>();
builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});



builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseRouting();


app.MapControllerRoute(
        name: "Search",
        pattern: "Search/{query?}",
        defaults: new { controller = "Home", action = "Search" });


// Outras rotas



app.MapControllerRoute(
    name: "Search",
    pattern: "Search/{query?}",
    defaults: new { controller = "Home", action = "Search" });


app.MapControllerRoute(
    name: "Products",
    pattern: "Products",
    defaults: new { controller = "Products", action = "IndexProducts" });


app.MapControllerRoute(
    name: "ImprimirNumeros",
    pattern: "Products/ImprimirNumeros",
    defaults: new { controller = "Products", action = "ImprimirNumeros" }
);



app.MapControllerRoute(
    name: "CustomersCreate",
    pattern: "Customers/Create",
    defaults: new { controller = "Customers", action = "Create" });


app.MapControllerRoute(
    name: "Customers",
    pattern: "Customers",
    defaults: new { controller = "Customers", action = "IndexCustomers" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home1}/{id?}");




app.Run();
