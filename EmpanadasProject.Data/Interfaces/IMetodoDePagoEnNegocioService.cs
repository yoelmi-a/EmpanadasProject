using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Base;

namespace EmpanadasProject.Data.Interfaces
{
    public interface IMetodoDePagoEnNegocioService
    {
        Task<OperationResult<MetodoDePagoEnNegocio>> AgregarMetodoPagoAsync(MetodoDePagoEnNegocio metodoPagoEnNegocio);
        Task<OperationResult<MetodoDePagoEnNegocio>> RemoverMetodoPagoAsync(string metodoId, string negocioId);
        Task<OperationResult<IEnumerable<MetodoDePagoEnNegocio>>> ObtenerPorNegocioAsync(string negocioId);
    }
}
