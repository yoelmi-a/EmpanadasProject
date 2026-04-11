using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Base;

namespace EmpanadasProject.Data.Interfaces
{
    public interface IProductoEnCarritoService
    {
        Task<OperationResult<ProductoEnCarrito>> AgregarAsync(ProductoEnCarrito producto);
        Task<OperationResult<bool>> LimpiarCarritoAsync(int carritoId);
        Task<OperationResult<ProductoEnCarrito>> RemoverAsync(int id);
        Task<OperationResult<IEnumerable<ProductoEnCarrito>>> ObtenerPorCarritoAsync(int carritoId);
    }
}
