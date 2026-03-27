using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class MetodoDePagoEnNegocioService : IMetodoDePagoEnNegocioService
    {
        private readonly EmpanadasContext _context;

        public MetodoDePagoEnNegocioService(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<MetodoDePagoEnNegocio>> AgregarMetodoPagoAsync(MetodoDePagoEnNegocio metodoPagoEnNegocio)
        {
            var result = new OperationResult<MetodoDePagoEnNegocio>();
            try
            {
                var exists = await _context.MetodosDePagoEnNegocio.FindAsync(metodoPagoEnNegocio.MetodoId, metodoPagoEnNegocio.NegocioId);
                if (exists != null)
                {
                    result.Success = false;
                    result.Message = "El método de pago ya existe en este negocio.";
                    return result;
                }

                _context.MetodosDePagoEnNegocio.Add(metodoPagoEnNegocio);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Método de pago agregado al negocio exitosamente.";
                result.Data = metodoPagoEnNegocio;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error agregando el método de pago al negocio: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult<MetodoDePagoEnNegocio>> RemoverMetodoPagoAsync(string metodoId, string negocioId)
        {
            var result = new OperationResult<MetodoDePagoEnNegocio>();
            try
            {
                var metodoPago = await _context.MetodosDePagoEnNegocio.FindAsync(metodoId, negocioId);
                if (metodoPago == null)
                {
                    result.Success = false;
                    result.Message = "El método de pago no se encuentra en este negocio.";
                    return result;
                }

                _context.MetodosDePagoEnNegocio.Remove(metodoPago);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Método de pago removido del negocio exitosamente.";
                result.Data = metodoPago;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error removiendo el método de pago del negocio: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<MetodoDePagoEnNegocio>>> ObtenerPorNegocioAsync(string negocioId)
        {
            var result = new OperationResult<IEnumerable<MetodoDePagoEnNegocio>>();
            try
            {
                var metodos = await _context.MetodosDePagoEnNegocio
                                            .Where(m => m.NegocioId == negocioId)
                                            .ToListAsync();

                result.Success = true;
                result.Message = "Métodos de pago del negocio obtenidos exitosamente.";
                result.Data = metodos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error obteniendo los métodos de pago del negocio: {ex.Message}";
            }
            return result;
        }
    }
}
