using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PrimeBeautyMVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar DbContext con MySQL
builder.Services.AddDbContext<PrimebeautyContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("PrimeBeauty")));

// Agregar servicios para sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // duración sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Agregar HttpContextAccessor para usar sesión en vistas y controladores
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Importante: colocar UseSession antes de UseAuthorization
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
