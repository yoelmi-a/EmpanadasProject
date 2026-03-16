using EmpanadasProject.Data.Entities.Productos;

namespace EmpanadasProject.Data.Entities.Pedidos
{
    public class ProductoEnPedido : ProductoBase
    {
        public int PedidoId { get; set; }
    }
}
