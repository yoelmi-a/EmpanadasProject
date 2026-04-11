using EmpanadasProject.Data.Context;
using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces;
using EmpanadasProject.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class PromocionService : IPromocionService
    {
        private readonly AppDbContext _context;

        public PromocionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Promocion>> AddAsync(Promocion promocion)
        {
            try
            {
                _context.Promociones.Add(promocion);
                await _context.SaveChangesAsync();
                return OperationResult<Promocion>.Exitoso(promocion);
            }
            catch (Exception ex)
            {
                return OperationResult<Promocion>.Fallido($"Error agregando la promoción: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<Promocion>>> GetAllAsync()
        {
            try
            {
                var promociones = await _context.Promociones.ToListAsync();
                return OperationResult<IEnumerable<Promocion>>.Exitoso(promociones);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Promocion>>.Fallido($"Error obteniendo las promociones: {ex.Message}");
            }
        }

        public async Task<OperationResult<Promocion>> GetByIdAsync(int id)
        {
            try
            {
                var promocion = await _context.Promociones.FindAsync(id);
                if (promocion == null)
                {
                    return OperationResult<Promocion>.Fallido("Promoción no encontrada.");
                }
                return OperationResult<Promocion>.Exitoso(promocion);
            }
            catch (Exception ex)
            {
                return OperationResult<Promocion>.Fallido($"Error obteniendo la promoción: {ex.Message}");
            }
        }

        public async Task<OperationResult<Promocion>> UpdateAsync(Promocion promocion)
        {
            try
            {
                var existing = await _context.Promociones.FindAsync(promocion.Id);
                if (existing == null)
                {
                    return OperationResult<Promocion>.Fallido("Promoción no encontrada.");
                }
                
                existing.Nombre = promocion.Nombre;
                existing.Descuento = promocion.Descuento;
                existing.TipoDescuento = promocion.TipoDescuento;
                existing.FechaInicio = promocion.FechaInicio;
                existing.FechaFin = promocion.FechaFin;
                existing.EstaActiva = promocion.EstaActiva;
                existing.ActualizadoEn = DateTime.UtcNow;

                _context.Promociones.Update(existing);
                await _context.SaveChangesAsync();
                
                return OperationResult<Promocion>.Exitoso(existing);
            }
            catch (Exception ex)
            {
                return OperationResult<Promocion>.Fallido($"Error actualizando la promoción: {ex.Message}");
            }
        }

        public async Task<OperationResult<Promocion>> DeleteAsync(int id)
        {
            try
            {
                var promocion = await _context.Promociones.FindAsync(id);
                if (promocion == null)
                {
                    return OperationResult<Promocion>.Fallido("Promoción no encontrada.");
                }
                _context.Promociones.Remove(promocion);
                await _context.SaveChangesAsync();
                
                return OperationResult<Promocion>.Exitoso(promocion);
            }
            catch (Exception ex)
            {
                return OperationResult<Promocion>.Fallido($"Error eliminando la promoción: {ex.Message}");
            }
        }
    }
}
