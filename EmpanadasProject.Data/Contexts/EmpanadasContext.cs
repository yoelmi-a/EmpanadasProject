using EmpanadasProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Contexts
{
    public class EmpanadasContext : DbContext
    {
        public EmpanadasContext(DbContextOptions<EmpanadasContext> options) : base(options)
        {            
        }

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<ItemPedido> ItemsPedido => Set<ItemPedido>();
        public DbSet<Promocion> Promociones => Set<Promocion>();
        public DbSet<ProductoEnCarrito> ProductosEnCarrito => Set<ProductoEnCarrito>();
        public DbSet<MetodoDePagoEnNegocio> MetodosDePagoEnNegocio => Set<MetodoDePagoEnNegocio>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MetodoDePagoEnNegocio>()
                .HasKey(m => new { m.MetodoId, m.NegocioId });
        }
    }
}
