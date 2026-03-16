using EmpanadasProject.Data.Entities.Productos;
using EmpanadasProject.Data.OperationResult;

namespace EmpanadasProject.Data.Interfaces.Productos
{
    public interface IProductoService
    {
        public Task<OperationResult<Producto>> AddProductoAsync(Producto producto);
        public Task<OperationResult<Producto>> GetProductoByIdAsync(int id);
        public Task<OperationResult<IEnumerable<Producto>>> GetAllProductosAsync();
        public Task<OperationResult<Producto>> UpdateProductoAsync(Producto producto);
        public Task<OperationResult<Producto>> DeleteProductoAsync(int id);
    }
}
