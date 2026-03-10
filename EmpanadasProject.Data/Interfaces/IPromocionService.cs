using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.OperationResult;

namespace EmpanadasProject.Data.Interfaces
{
    public interface IPromocionService
    {
        Task<OperationResult<Promocion>> AddAsync(Promocion promocion);
        Task<OperationResult<IEnumerable<Promocion>>> GetAllAsync();
        Task<OperationResult<Promocion>> GetByIdAsync(int id);
        Task<OperationResult<Promocion>> UpdateAsync(Promocion promocion);
        Task<OperationResult<Promocion>> DeleteAsync(int id);
    }
}
