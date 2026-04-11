using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Base;

namespace EmpanadasProject.Data.Interfaces.Usuario
{
    public interface IMetodoPagoService
    {
        public Task<OperationResult<MetodoPago>> AddMetodoPagoAsync(MetodoPago metodoPago);
        public Task<OperationResult<MetodoPago>> GetMetodoPagoByIdAsync(int Id);
        public Task<OperationResult<IEnumerable<MetodoPago>>> GetAllMetodoPago();
        public Task<OperationResult<MetodoPago>> DeleteMetodoPago(int Id);
        public Task<OperationResult<MetodoPago>> UpdateMetodoPago(MetodoPago metodoPago);
    }
}
