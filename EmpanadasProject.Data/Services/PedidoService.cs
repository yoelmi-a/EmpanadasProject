using EmpanadasProject.Data.Context;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces.Pedidos;
using EmpanadasProject.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Pedido>> AddPedidoAsync(Pedido pedido)
        {
            try
            {
                pedido.FechaPedido = DateTime.UtcNow;
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                return OperationResult<Pedido>.Exitoso(pedido);
            }
            catch (Exception ex)
            {
                return OperationResult<Pedido>.Fallido($"Error al agregar el pedido: {ex.Message}");
            }
        }

        public async Task<OperationResult<Pedido>> GetPedidoByIdAsync(int id)
        {
            try
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.Items)
                    .Include(p => p.Historiales)
                    .FirstOrDefaultAsync(p => p.Id == id);
                    
                if (pedido == null)
                {
                    return OperationResult<Pedido>.Fallido("Pedido no encontrado.");
                }
                return OperationResult<Pedido>.Exitoso(pedido);
            }
            catch (Exception ex)
            {
                return OperationResult<Pedido>.Fallido($"Error al obtener el pedido: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<Pedido>>> GetAllPedidosAsync()
        {
            try
            {
                var pedidos = await _context.Pedidos.ToListAsync();
                return OperationResult<IEnumerable<Pedido>>.Exitoso(pedidos);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Pedido>>.Fallido($"Error al obtener los pedidos: {ex.Message}");
            }
        }

        public async Task<OperationResult<Pedido>> UpdatePedidoAsync(Pedido pedido)
        {
            try
            {
                var existingPedido = await _context.Pedidos.FindAsync(pedido.Id);
                if (existingPedido == null)
                {
                    return OperationResult<Pedido>.Fallido("Pedido no encontrado.");
                }
                
                existingPedido.Estado = pedido.Estado;
                existingPedido.Total = pedido.Total;
                existingPedido.Subtotal = pedido.Subtotal;
                existingPedido.Impuestos = pedido.Impuestos;
                existingPedido.MetodoEntrega = pedido.MetodoEntrega;
                existingPedido.ActualizadoEn = DateTime.UtcNow;

                _context.Pedidos.Update(existingPedido);
                await _context.SaveChangesAsync();
                return OperationResult<Pedido>.Exitoso(existingPedido);
            }
            catch (Exception ex)
            {
                return OperationResult<Pedido>.Fallido($"Error al actualizar el pedido: {ex.Message}");
            }
        }

        public async Task<OperationResult<Pedido>> DeletePedidoAsync(int id)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);
                if (pedido == null)
                {
                    return OperationResult<Pedido>.Fallido("Pedido no encontrado.");
                }
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
                return OperationResult<Pedido>.Exitoso(pedido);
            }
            catch (Exception ex)
            {
                return OperationResult<Pedido>.Fallido($"Error al eliminar el pedido: {ex.Message}");
            }
        }
    }
}
