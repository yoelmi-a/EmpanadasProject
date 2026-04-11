using EmpanadasProject.Data.Context;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces.Productos;
using EmpanadasProject.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Producto>> AddProductoAsync(Producto producto)
        {
            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return OperationResult<Producto>.Exitoso(producto);
            }
            catch (Exception ex)
            {
                return OperationResult<Producto>.Fallido($"Error al agregar el producto: {ex.Message}");
            }
        }

        public async Task<OperationResult<Producto>> GetProductoByIdAsync(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return OperationResult<Producto>.Fallido("Producto no encontrado.");
                }
                return OperationResult<Producto>.Exitoso(producto);
            }
            catch (Exception ex)
            {
                return OperationResult<Producto>.Fallido($"Error al obtener el producto: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<Producto>>> GetAllProductosAsync()
        {
            try
            {
                var productos = await _context.Productos.ToListAsync();
                return OperationResult<IEnumerable<Producto>>.Exitoso(productos);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Producto>>.Fallido($"Error al obtener los productos: {ex.Message}");
            }
        }

        public async Task<OperationResult<Producto>> UpdateProductoAsync(Producto producto)
        {
            try
            {
                var existingProducto = await _context.Productos.FindAsync(producto.Id);
                if (existingProducto == null)
                {
                    return OperationResult<Producto>.Fallido("Producto no encontrado.");
                }
                existingProducto.Nombre = producto.Nombre;
                existingProducto.Descripcion = producto.Descripcion;
                existingProducto.Precio = producto.Precio;
                existingProducto.EsDisponible = producto.EsDisponible;
                existingProducto.Categoria = producto.Categoria;
                existingProducto.ActualizadoEn = DateTime.UtcNow;

                _context.Productos.Update(existingProducto);
                await _context.SaveChangesAsync();
                return OperationResult<Producto>.Exitoso(existingProducto);
            }
            catch (Exception ex)
            {
                return OperationResult<Producto>.Fallido($"Error al actualizar el producto: {ex.Message}");
            }
        }

        public async Task<OperationResult<Producto>> DeleteProductoAsync(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return OperationResult<Producto>.Fallido("Producto no encontrado.");
                }
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                return OperationResult<Producto>.Exitoso(producto);
            }
            catch (Exception ex)
            {
                return OperationResult<Producto>.Fallido($"Error al eliminar el producto: {ex.Message}");
            }
        }
    }
}
