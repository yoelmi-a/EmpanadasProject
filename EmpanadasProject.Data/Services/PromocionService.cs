using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class PromocionService : IPromocionService
    {
        private readonly EmpanadasContext _context;

        public PromocionService(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Promocion>> AddAsync(Promocion promocion)
        {
            var result = new OperationResult<Promocion>();
            try
            {
                _context.Promociones.Add(promocion);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Promoción agregada exitosamente.";
                result.Data = promocion;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error agregando la promoción: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<Promocion>>> GetAllAsync()
        {
            var result = new OperationResult<IEnumerable<Promocion>>();
            try
            {
                var promociones = await _context.Promociones.ToListAsync();
                result.Success = true;
                result.Message = "Promociones obtenidas exitosamente.";
                result.Data = promociones;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error obteniendo las promociones: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult<Promocion>> GetByIdAsync(int id)
        {
            var result = new OperationResult<Promocion>();
            try
            {
                var promocion = await _context.Promociones.FindAsync(id);
                if (promocion == null)
                {
                    result.Success = false;
                    result.Message = "Promoción no encontrada.";
                    return result;
                }
                result.Success = true;
                result.Message = "Promoción obtenida exitosamente.";
                result.Data = promocion;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error obteniendo la promoción: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult<Promocion>> UpdateAsync(Promocion promocion)
        {
            var result = new OperationResult<Promocion>();
            try
            {
                var existing = await _context.Promociones.FindAsync(promocion.Id);
                if (existing == null)
                {
                    result.Success = false;
                    result.Message = "Promoción no encontrada.";
                    return result;
                }
                
                existing.Nombre = promocion.Nombre;
                existing.Descuento = promocion.Descuento;
                existing.Fecha = promocion.Fecha;

                _context.Promociones.Update(existing);
                await _context.SaveChangesAsync();
                
                result.Success = true;
                result.Message = "Promoción actualizada exitosamente.";
                result.Data = existing;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error actualizando la promoción: {ex.Message}";
            }
            return result;
        }

        public async Task<OperationResult<Promocion>> DeleteAsync(int id)
        {
            var result = new OperationResult<Promocion>();
            try
            {
                var promocion = await _context.Promociones.FindAsync(id);
                if (promocion == null)
                {
                    result.Success = false;
                    result.Message = "Promoción no encontrada.";
                    return result;
                }
                _context.Promociones.Remove(promocion);
                await _context.SaveChangesAsync();
                
                result.Success = true;
                result.Message = "Promoción eliminada exitosamente.";
                result.Data = promocion;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error eliminando la promoción: {ex.Message}";
            }
            return result;
        }
    }
}
