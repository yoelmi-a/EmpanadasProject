using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class ProductoEnCarritoService : IProductoEnCarritoService
    {
        private readonly EmpanadasContext _context;

        public ProductoEnCarritoService(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<ProductoEnCarrito>> AgregarAsync(ProductoEnCarrito producto)
        {
            var result = new OperationResult<ProductoEnCarrito>();
            try
            {
                _context.ProductosEnCarrito.Add(producto);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Producto agregado al carrito exitosamente.";
                result.Data = producto;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error agregando el producto al carrito: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult<bool>> LimpiarCarritoAsync(int carritoId)
        {
            var result = new OperationResult<bool>();
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

                result.Success = true;
                result.Message = "Carrito limpiado exitosamente.";
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error limpiando el carrito: {ex.Message}";
                result.Data = false;
            }
            return result;
        }

        public async Task<OperationResult<ProductoEnCarrito>> RemoverAsync(int id)
        {
            var result = new OperationResult<ProductoEnCarrito>();
            try
            {
                var producto = await _context.ProductosEnCarrito.FindAsync(id);
                if (producto == null)
                {
                    result.Success = false;
                    result.Message = "Producto no encontrado en el carrito.";
                    return result;
                }
                
                _context.ProductosEnCarrito.Remove(producto);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Producto removido del carrito exitosamente.";
                result.Data = producto;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error removiendo el producto del carrito: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<ProductoEnCarrito>>> ObtenerPorCarritoAsync(int carritoId)
        {
            var result = new OperationResult<IEnumerable<ProductoEnCarrito>>();
            try
            {
                var productos = await _context.ProductosEnCarrito
                                              .Where(p => p.CarritoId == carritoId)
                                              .ToListAsync();

                result.Success = true;
                result.Message = "Productos del carrito obtenidos exitosamente.";
                result.Data = productos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error obteniendo los productos del carrito: {ex.Message}";
            }
            return result;
        }
    }
}
