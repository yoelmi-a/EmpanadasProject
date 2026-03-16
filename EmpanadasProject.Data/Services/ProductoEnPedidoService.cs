using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Pedidos;
using EmpanadasProject.Data.Interfaces.Pedidos;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class ProductoEnPedidoService : IProductoEnPedidoService
    {
        private readonly EmpanadasContext _context;

        public ProductoEnPedidoService(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<ProductoEnPedido>> AddProductoEnPedidoAsync(ProductoEnPedido productoEnPedido)
        {
            OperationResult<ProductoEnPedido> result = new OperationResult<ProductoEnPedido>();
            try
            {
                _context.ProductoEnPedidos.Add(productoEnPedido);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Producto agregado al pedido exitosamente.";
                result.Data = productoEnPedido;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al agregar el producto al pedido: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<ProductoEnPedido>> GetProductoEnPedidoByIdAsync(int id)
        {
            OperationResult<ProductoEnPedido> result = new OperationResult<ProductoEnPedido>();
            try
            {
                var productoEnPedido = await _context.ProductoEnPedidos.FindAsync(id);
                if (productoEnPedido == null)
                {
                    result.Success = false;
                    result.Message = "Producto en pedido no encontrado.";
                    result.Data = null;
                    return result;
                }
                result.Success = true;
                result.Message = "Producto en pedido obtenido exitosamente.";
                result.Data = productoEnPedido;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener el producto en pedido: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<ProductoEnPedido>>> GetProductosByPedidoIdAsync(int pedidoId)
        {
            OperationResult<IEnumerable<ProductoEnPedido>> result = new OperationResult<IEnumerable<ProductoEnPedido>>();
            try
            {
                var productos = await _context.ProductoEnPedidos.Where(p => p.PedidoId == pedidoId).ToListAsync();
                result.Success = true;
                result.Message = "Productos del pedido obtenidos exitosamente.";
                result.Data = productos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener los productos del pedido: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<ProductoEnPedido>> DeleteProductoEnPedidoAsync(int id)
        {
            OperationResult<ProductoEnPedido> result = new OperationResult<ProductoEnPedido>();
            try
            {
                var productoEnPedido = await _context.ProductoEnPedidos.FindAsync(id);
                if (productoEnPedido == null)
                {
                    result.Success = false;
                    result.Message = "Producto en pedido no encontrado.";
                    result.Data = null;
                    return result;
                }
                _context.ProductoEnPedidos.Remove(productoEnPedido);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Producto en pedido eliminado exitosamente.";
                result.Data = productoEnPedido;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al eliminar el producto en pedido: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
    }
}
