using EmpanadasProject.Data.Context;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces;
using EmpanadasProject.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class MetodoDePagoEnNegocioService : IMetodoDePagoEnNegocioService
    {
        private readonly AppDbContext _context;

        public MetodoDePagoEnNegocioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<MetodoDePagoEnNegocio>> AgregarMetodoPagoAsync(MetodoDePagoEnNegocio metodoPagoEnNegocio)
        {
            try
            {
                var exists = await _context.Set<MetodoDePagoEnNegocio>().FindAsync(metodoPagoEnNegocio.MetodoId, metodoPagoEnNegocio.NegocioId);
                if (exists != null)
                {
                    return OperationResult<MetodoDePagoEnNegocio>.Fallido("El método de pago ya existe en este negocio.");
                }

                _context.Set<MetodoDePagoEnNegocio>().Add(metodoPagoEnNegocio);
                await _context.SaveChangesAsync();

                return OperationResult<MetodoDePagoEnNegocio>.Exitoso(metodoPagoEnNegocio);
            }
            catch (Exception ex)
            {
                return OperationResult<MetodoDePagoEnNegocio>.Fallido($"Error agregando el método de pago al negocio: {ex.Message}");
            }
        }

        public async Task<OperationResult<MetodoDePagoEnNegocio>> RemoverMetodoPagoAsync(string metodoId, string negocioId)
        {
            try
            {
                var metodoPago = await _context.Set<MetodoDePagoEnNegocio>().FindAsync(metodoId, negocioId);
                if (metodoPago == null)
                {
                    return OperationResult<MetodoDePagoEnNegocio>.Fallido("El método de pago no se encuentra en este negocio.");
                }

                _context.Set<MetodoDePagoEnNegocio>().Remove(metodoPago);
                await _context.SaveChangesAsync();

                return OperationResult<MetodoDePagoEnNegocio>.Exitoso(metodoPago);
            }
            catch (Exception ex)
            {
                return OperationResult<MetodoDePagoEnNegocio>.Fallido($"Error removiendo el método de pago del negocio: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<MetodoDePagoEnNegocio>>> ObtenerPorNegocioAsync(string negocioId)
        {
            try
            {
                var metodos = await _context.Set<MetodoDePagoEnNegocio>()
                                            .Where(m => m.NegocioId == negocioId)
                                            .ToListAsync();

                return OperationResult<IEnumerable<MetodoDePagoEnNegocio>>.Exitoso(metodos);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<MetodoDePagoEnNegocio>>.Fallido($"Error obteniendo los métodos de pago del negocio: {ex.Message}");
            }
        }
    }
}
