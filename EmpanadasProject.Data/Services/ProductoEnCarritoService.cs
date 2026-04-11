using EmpanadasProject.Data.Context;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces;
using EmpanadasProject.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class ProductoEnCarritoService : IProductoEnCarritoService
    {
        private readonly AppDbContext _context;

        public ProductoEnCarritoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<ProductoEnCarrito>> AgregarAsync(ProductoEnCarrito producto)
        {
            try
            {
                _context.ProductosEnCarrito.Add(producto);
                await _context.SaveChangesAsync();
                return OperationResult<ProductoEnCarrito>.Exitoso(producto);
            }
            catch (Exception ex)
            {
                return OperationResult<ProductoEnCarrito>.Fallido($"Error agregando el producto al carrito: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> LimpiarCarritoAsync(int carritoId)
        {
            try
            {
                var productos = await _context.ProductosEnCarrito
                                              .Where(p => p.CarritoId == carritoId)
                                              .ToListAsync();
                
                if (productos.Any())
                {
                    _context.ProductosEnCarrito.RemoveRange(productos);
                    await _context.SaveChangesAsync();
                }

                return OperationResult<bool>.Exitoso(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fallido($"Error limpiando el carrito: {ex.Message}");
            }
        }

        public async Task<OperationResult<ProductoEnCarrito>> RemoverAsync(int id)
        {
            try
            {
                var producto = await _context.ProductosEnCarrito.FindAsync(id);
                if (producto == null)
                {
                    return OperationResult<ProductoEnCarrito>.Fallido("Producto no encontrado en el carrito.");
                }
                
                _context.ProductosEnCarrito.Remove(producto);
                await _context.SaveChangesAsync();

                return OperationResult<ProductoEnCarrito>.Exitoso(producto);
            }
            catch (Exception ex)
            {
                return OperationResult<ProductoEnCarrito>.Fallido($"Error removiendo el producto del carrito: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<ProductoEnCarrito>>> ObtenerPorCarritoAsync(int carritoId)
        {
            try
            {
                var productos = await _context.ProductosEnCarrito
                                              .Where(p => p.CarritoId == carritoId)
                                              .ToListAsync();

                return OperationResult<IEnumerable<ProductoEnCarrito>>.Exitoso(productos);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<ProductoEnCarrito>>.Fallido($"Error obteniendo los productos del carrito: {ex.Message}");
            }
        }
    }
}
