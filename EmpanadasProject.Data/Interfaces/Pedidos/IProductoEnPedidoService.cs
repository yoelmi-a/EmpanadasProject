using EmpanadasProject.Data.Entities.Pedidos;
using EmpanadasProject.Data.OperationResult;

namespace EmpanadasProject.Data.Interfaces.Pedidos
{
    public interface IProductoEnPedidoService
    {
        public Task<OperationResult<ProductoEnPedido>> AddProductoEnPedidoAsync(ProductoEnPedido productoEnPedido);
        public Task<OperationResult<ProductoEnPedido>> GetProductoEnPedidoByIdAsync(int id);
        public Task<OperationResult<IEnumerable<ProductoEnPedido>>> GetProductosByPedidoIdAsync(int pedidoId);
        public Task<OperationResult<ProductoEnPedido>> DeleteProductoEnPedidoAsync(int id);
    }
}
