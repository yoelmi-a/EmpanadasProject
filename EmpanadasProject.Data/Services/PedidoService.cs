using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Pedidos;
using EmpanadasProject.Data.Interfaces.Pedidos;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly EmpanadasContext _context;

        public PedidoService(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Pedido>> AddPedidoAsync(Pedido pedido)
        {
            OperationResult<Pedido> result = new OperationResult<Pedido>();
            try
            {
                pedido.Fecha = DateTime.Now;
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Pedido agregado exitosamente.";
                result.Data = pedido;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al agregar el pedido: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<Pedido>> GetPedidoByIdAsync(int id)
        {
            OperationResult<Pedido> result = new OperationResult<Pedido>();
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);
                if (pedido == null)
                {
                    result.Success = false;
                    result.Message = "Pedido no encontrado.";
                    result.Data = null;
                    return result;
                }
                result.Success = true;
                result.Message = "Pedido obtenido exitosamente.";
                result.Data = pedido;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener el pedido: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<Pedido>>> GetAllPedidosAsync()
        {
            OperationResult<IEnumerable<Pedido>> result = new OperationResult<IEnumerable<Pedido>>();
            try
            {
                var pedidos = await _context.Pedidos.ToListAsync();
                result.Success = true;
                result.Message = "Pedidos obtenidos exitosamente.";
                result.Data = pedidos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener los pedidos: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<Pedido>> UpdatePedidoAsync(Pedido pedido)
        {
            OperationResult<Pedido> result = new OperationResult<Pedido>();
            try
            {
                var existingPedido = await _context.Pedidos.FindAsync(pedido.Id);
                if (existingPedido == null)
                {
                    result.Success = false;
                    result.Message = "Pedido no encontrado.";
                    result.Data = null;
                    return result;
                }
                existingPedido.ClienteId = pedido.ClienteId;
                existingPedido.NegocioId = pedido.NegocioId;
                existingPedido.MetodoDePagoId = pedido.MetodoDePagoId;
                existingPedido.TotalPagado = pedido.TotalPagado;
                existingPedido.Estado = pedido.Estado;
                existingPedido.PromocionId = pedido.PromocionId;

                _context.Pedidos.Update(existingPedido);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Pedido actualizado exitosamente.";
                result.Data = existingPedido;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al actualizar el pedido: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<Pedido>> DeletePedidoAsync(int id)
        {
            OperationResult<Pedido> result = new OperationResult<Pedido>();
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);
                if (pedido == null)
                {
                    result.Success = false;
                    result.Message = "Pedido no encontrado.";
                    result.Data = null;
                    return result;
                }
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Pedido eliminado exitosamente.";
                result.Data = pedido;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al eliminar el pedido: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
    }
}
