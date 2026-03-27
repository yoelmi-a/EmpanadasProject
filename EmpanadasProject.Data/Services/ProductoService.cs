using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Productos;
using EmpanadasProject.Data.Interfaces.Productos;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class ProductoService : IProductoService
    {
        private readonly EmpanadasContext _context;

        public ProductoService(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Producto>> AddProductoAsync(Producto producto)
        {
            OperationResult<Producto> result = new OperationResult<Producto>();
            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Producto agregado exitosamente.";
                result.Data = producto;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al agregar el producto: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<Producto>> GetProductoByIdAsync(int id)
        {
            OperationResult<Producto> result = new OperationResult<Producto>();
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    result.Success = false;
                    result.Message = "Producto no encontrado.";
                    result.Data = null;
                    return result;
                }
                result.Success = true;
                result.Message = "Producto obtenido exitosamente.";
                result.Data = producto;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener el producto: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<Producto>>> GetAllProductosAsync()
        {
            OperationResult<IEnumerable<Producto>> result = new OperationResult<IEnumerable<Producto>>();
            try
            {
                var productos = await _context.Productos.ToListAsync();
                result.Success = true;
                result.Message = "Productos obtenidos exitosamente.";
                result.Data = productos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener los productos: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<Producto>> UpdateProductoAsync(Producto producto)
        {
            OperationResult<Producto> result = new OperationResult<Producto>();
            try
            {
                var existingProducto = await _context.Productos.FindAsync(producto.Id);
                if (existingProducto == null)
                {
                    result.Success = false;
                    result.Message = "Producto no encontrado.";
                    result.Data = null;
                    return result;
                }
                existingProducto.Nombre = producto.Nombre;
                existingProducto.Descripcion = producto.Descripcion;
                existingProducto.Precio = producto.Precio;
                existingProducto.Cantidad = producto.Cantidad;

                _context.Productos.Update(existingProducto);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Producto actualizado exitosamente.";
                result.Data = existingProducto;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al actualizar el producto: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<Producto>> DeleteProductoAsync(int id)
        {
            OperationResult<Producto> result = new OperationResult<Producto>();
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    result.Success = false;
                    result.Message = "Producto no encontrado.";
                    result.Data = null;
                    return result;
                }
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Producto eliminado exitosamente.";
                result.Data = producto;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al eliminar el producto: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
    }
}
