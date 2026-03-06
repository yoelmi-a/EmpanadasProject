using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.Repositories;
using EmpanadasProject.Data.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EmpanadasContext>(options => options.UseInMemoryDatabase("EmpanadasDb"));

builder.Services.AddScoped<IUsuarioService, UsuarioServiece>();
builder.Services.AddScoped<IRolUsuarioService, RolUsuarioService>();
builder.Services.AddScoped<IMetodoPagoService, MetodoPagoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
