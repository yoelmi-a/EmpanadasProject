using EmpanadasProject.Data.Context;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {
        private readonly AppDbContext _context;

        public MetodoPagoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<MetodoPago>> AddMetodoPagoAsync(MetodoPago metodoPago)
        {
            try
            {
                _context.Set<MetodoPago>().Add(metodoPago);
                await _context.SaveChangesAsync();
                return OperationResult<MetodoPago>.Exitoso(metodoPago);
            }
            catch (Exception ex) 
            {
                return OperationResult<MetodoPago>.Fallido($"Error agregando el método de pago: {ex.Message}");
            }
        }

        public async Task<OperationResult<MetodoPago>> DeleteMetodoPago(int Id)
        {
            try
            {
                var metodoPago = await _context.Set<MetodoPago>().FindAsync(Id);
                if (metodoPago == null)
                {
                    return OperationResult<MetodoPago>.Fallido("Método de pago no encontrado.");
                }
                _context.Set<MetodoPago>().Remove(metodoPago);
                await _context.SaveChangesAsync();
                return OperationResult<MetodoPago>.Exitoso(metodoPago);
            }
            catch (Exception ex)
            {
                return OperationResult<MetodoPago>.Fallido($"Error eliminando el método de pago: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<MetodoPago>>> GetAllMetodoPago()
        {
            try
            {
                var metodoPagos = await _context.Set<MetodoPago>().ToListAsync();
                return OperationResult<IEnumerable<MetodoPago>>.Exitoso(metodoPagos);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<MetodoPago>>.Fallido($"Error obteniendo los métodos de pago: {ex.Message}");
            }
        }

        public async Task<OperationResult<MetodoPago>> GetMetodoPagoByIdAsync(int Id)
        {
            try
            {
                var metodoPago = await _context.Set<MetodoPago>().FindAsync(Id);
                if (metodoPago == null)
                {
                    return OperationResult<MetodoPago>.Fallido("Método de pago no encontrado.");
                }
                return OperationResult<MetodoPago>.Exitoso(metodoPago);
            }
            catch (Exception ex)
            {
                return OperationResult<MetodoPago>.Fallido($"Error obteniendo el método de pago: {ex.Message}");
            }
        }

        public async Task<OperationResult<MetodoPago>> UpdateMetodoPago(MetodoPago metodoPago)
        {
            try
            {
                var existingMetodoPago = await _context.Set<MetodoPago>().FindAsync(metodoPago.Id);
                if (existingMetodoPago == null)
                {
                    return OperationResult<MetodoPago>.Fallido("Método de pago no encontrado.");
                }
                existingMetodoPago.Nombre = metodoPago.Nombre;
                _context.Set<MetodoPago>().Update(existingMetodoPago);
                await _context.SaveChangesAsync();
                return OperationResult<MetodoPago>.Exitoso(existingMetodoPago);
            }
            catch (Exception ex)
            {
                return OperationResult<MetodoPago>.Fallido($"Error actualizando el método de pago: {ex.Message}");
            }
        }
    }
}
