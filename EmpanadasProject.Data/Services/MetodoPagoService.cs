using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {
        private readonly EmpanadasContext _context;

        public MetodoPagoService(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<MetodoPago>> AddMetodoPagoAsync(MetodoPago metodoPago)
        {
            OperationResult<MetodoPago> result = new OperationResult<MetodoPago>();

            try
            {
                _context.MetodoPagos.Add(metodoPago);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Metodo de Pago agregado exitosamente.";
                result.Data = metodoPago;
            }
            catch (Exception ex) 
            {
                result.Success = false;
                result.Message = $"Error agregando el metodo de pago: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<MetodoPago>> DeleteMetodoPago(int Id)
        {
            OperationResult<MetodoPago> result = new OperationResult<MetodoPago>();
            try
            {
                var metodoPago = await _context.MetodoPagos.FindAsync(Id);
                if (metodoPago == null)
                {
                    result.Success = false;
                    result.Message = "Metodo de Pago no encontrado.";
                    result.Data = null;
                    return result;
                }
                _context.MetodoPagos.Remove(metodoPago);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Metodo de Pago eliminado exitosamente.";
                result.Data = metodoPago;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error eliminando el metodo de pago: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<MetodoPago>>> GetAllMetodoPago()
        {
            OperationResult<IEnumerable<MetodoPago>> result = new OperationResult<IEnumerable<MetodoPago>>();
            try
            {
                var metodoPagos = await _context.MetodoPagos.ToListAsync();
                result.Success = true;
                result.Message = "Metodos de Pago obtenidos exitosamente.";
                result.Data = metodoPagos;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error obteniendo los metodos de pago: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<MetodoPago>> GetMetodoPagoByIdAsync(int Id)
        {
            OperationResult<MetodoPago> result = new OperationResult<MetodoPago>();
            try
            {
                var metodoPago = await _context.MetodoPagos.FindAsync(Id);
                if (metodoPago == null)
                {
                    result.Success = false;
                    result.Message = "Metodo de Pago no encontrado.";
                    result.Data = null;
                    return result;
                }
                result.Success = true;
                result.Message = "Metodo de Pago obtenido exitosamente.";
                result.Data = metodoPago;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error obteniendo el metodo de pago: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<MetodoPago>> UpdateMetodoPago(MetodoPago metodoPago)
        {
            OperationResult<MetodoPago> result = new OperationResult<MetodoPago>();
            try
            {
                var existingMetodoPago = await _context.MetodoPagos.FindAsync(metodoPago.Id);
                if (existingMetodoPago == null)
                {
                    result.Success = false;
                    result.Message = "Metodo de Pago no encontrado.";
                    result.Data = null;
                    return result;
                }
                existingMetodoPago.Nombre = metodoPago.Nombre;
                _context.MetodoPagos.Update(existingMetodoPago);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Metodo de Pago actualizado exitosamente.";
                result.Data = existingMetodoPago;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error actualizando el metodo de pago: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
    }
}
