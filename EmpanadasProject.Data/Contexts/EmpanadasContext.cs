using EmpanadasProject.Data.Entities.Pedidos;
using EmpanadasProject.Data.Entities.Productos;
using EmpanadasProject.Data.Entities.Usuario;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Contexts
{
    public class EmpanadasContext : DbContext
    {
        public EmpanadasContext(DbContextOptions<EmpanadasContext> options) : base(options)
        {            
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<RolUsuario> RolUsuarios { get; set; }
        public DbSet<MetodoPago> MetodoPagos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ProductoEnPedido> ProductoEnPedidos { get; set; }
    }
}
