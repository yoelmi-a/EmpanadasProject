using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Base;

namespace EmpanadasProject.Data.Interfaces.Pedidos
{
    public interface IProductoEnPedidoService
    {
        public Task<OperationResult<ItemPedido>> AddProductoEnPedidoAsync(ItemPedido productoEnPedido);
        public Task<OperationResult<ItemPedido>> GetProductoEnPedidoByIdAsync(int id);
        public Task<OperationResult<IEnumerable<ItemPedido>>> GetProductosByPedidoIdAsync(int pedidoId);
        public Task<OperationResult<ItemPedido>> DeleteProductoEnPedidoAsync(int id);
    }
}
