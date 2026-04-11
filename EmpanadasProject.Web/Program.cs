using EmpanadasProject.Data.Context;
using EmpanadasProject.Web.Models;
using EmpanadasProject.Web.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Base de Datos InMemory
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseInMemoryDatabase("DMendezDb"));

// ASP.NET Core Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
{
    opciones.Password.RequiredLength = 8;
    opciones.Password.RequireUppercase = true;
    opciones.Password.RequireDigit = true;
    opciones.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>();

// Configuración de Cookies
builder.Services.ConfigureApplicationCookie(opciones =>
{
    opciones.LoginPath = "/Account/Login";
    opciones.AccessDeniedPath = "/Account/AccessDenied";
});

// Configuración de Opciones
builder.Services.Configure<OpcionesAdminPorDefecto>(
    builder.Configuration.GetSection("DefaultAdmin"));

// Registrar Servicios de Semilla
builder.Services.AddScoped<SemillaAdminPorDefecto>();

var app = builder.Build();

// Ejecutar Semilla de Administrador
using (var scope = app.Services.CreateScope())
{
    var semilla = scope.ServiceProvider.GetRequiredService<SemillaAdminPorDefecto>();
    var resultado = await semilla.EjecutarAsync();
    if (!resultado.EsExitoso)
    {
        // El error ya se logueó dentro de la semilla
        Console.WriteLine($"ADVERTENCIA: Falló la semilla inicial: {resultado.MensajeError}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
