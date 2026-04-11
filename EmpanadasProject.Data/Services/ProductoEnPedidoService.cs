using EmpanadasProject.Data.Context;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces.Pedidos;
using EmpanadasProject.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class ProductoEnPedidoService : IProductoEnPedidoService
    {
        private readonly AppDbContext _context;

        public ProductoEnPedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<ItemPedido>> AddProductoEnPedidoAsync(ItemPedido productoEnPedido)
        {
            try
            {
                _context.ItemsPedido.Add(productoEnPedido);
                await _context.SaveChangesAsync();
                return OperationResult<ItemPedido>.Exitoso(productoEnPedido);
            }
            catch (Exception ex)
            {
                return OperationResult<ItemPedido>.Fallido($"Error al agregar el producto al pedido: {ex.Message}");
            }
        }

        public async Task<OperationResult<ItemPedido>> GetProductoEnPedidoByIdAsync(int id)
        {
            try
            {
                var productoEnPedido = await _context.ItemsPedido.FindAsync(id);
                if (productoEnPedido == null)
                {
                    return OperationResult<ItemPedido>.Fallido("Producto en pedido no encontrado.");
                }
                return OperationResult<ItemPedido>.Exitoso(productoEnPedido);
            }
            catch (Exception ex)
            {
                return OperationResult<ItemPedido>.Fallido($"Error al obtener el producto en pedido: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<ItemPedido>>> GetProductosByPedidoIdAsync(int pedidoId)
        {
            try
            {
                var productos = await _context.ItemsPedido.Where(p => p.PedidoId == pedidoId).ToListAsync();
                return OperationResult<IEnumerable<ItemPedido>>.Exitoso(productos);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<ItemPedido>>.Fallido($"Error al obtener los productos del pedido: {ex.Message}");
            }
        }

        public async Task<OperationResult<ItemPedido>> DeleteProductoEnPedidoAsync(int id)
        {
            try
            {
                var productoEnPedido = await _context.ItemsPedido.FindAsync(id);
                if (productoEnPedido == null)
                {
                    return OperationResult<ItemPedido>.Fallido("Producto en pedido no encontrado.");
                }
                _context.ItemsPedido.Remove(productoEnPedido);
                await _context.SaveChangesAsync();
                return OperationResult<ItemPedido>.Exitoso(productoEnPedido);
            }
            catch (Exception ex)
            {
                return OperationResult<ItemPedido>.Fallido($"Error al eliminar el producto en pedido: {ex.Message}");
            }
        }
    }
}
