using EmpanadasProject.Data.Entities;
﻿using EmpanadasProject.Data.Entities.Pedidos;
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
        public DbSet<Negocio> Negocios { get; set; }
        public DbSet<ProductoEnNegocio> ProductosEnNegocio { get; set; }
        public DbSet<ProductoBase> ProductosBase { get; set; }
        public DbSet<Promocion> Promociones { get; set; }
        public DbSet<ProductoEnCarrito> ProductosEnCarrito { get; set; }
        public DbSet<MetodoDePagoEnNegocio> MetodosDePagoEnNegocio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MetodoDePagoEnNegocio>()
                .HasKey(m => new { m.MetodoId, m.NegocioId });
        }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ProductoEnPedido> ProductoEnPedidos { get; set; }
    }
}
