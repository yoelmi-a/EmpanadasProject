using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Base;

namespace EmpanadasProject.Data.Interfaces.Pedidos
{
    public interface IPedidoService
    {
        public Task<OperationResult<Pedido>> AddPedidoAsync(Pedido pedido);
        public Task<OperationResult<Pedido>> GetPedidoByIdAsync(int id);
        public Task<OperationResult<IEnumerable<Pedido>>> GetAllPedidosAsync();
        public Task<OperationResult<Pedido>> UpdatePedidoAsync(Pedido pedido);
        public Task<OperationResult<Pedido>> DeletePedidoAsync(int id);
    }
}
