using EmpanadasProject.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EmpanadasProject.Data.Context
{
    /// <summary>
    /// Contexto de la base de datos de la aplicación.
    /// </summary>
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<ItemPedido> ItemsPedido => Set<ItemPedido>();
        public DbSet<HistorialEstado> HistorialesEstado => Set<HistorialEstado>();
        public DbSet<Promocion> Promociones => Set<Promocion>();
        public DbSet<ProductoEnCarrito> ProductosEnCarrito => Set<ProductoEnCarrito>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Todas las configuraciones se pueden aplicar desde el ensamblado si se desea separar en clases IEntityTypeConfiguration
            // builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
