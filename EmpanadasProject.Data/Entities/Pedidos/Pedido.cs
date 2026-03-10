using EmpanadasProject.Data.Entities.BaseEntity;

namespace EmpanadasProject.Data.Entities.Pedidos
{
    public class Pedido : BaseEntity.BaseEntity
    {
        public int ClienteId { get; set; }
        public int NegocioId { get; set; }
        public int MetodoDePagoId { get; set; }
        public decimal TotalPagado { get; set; }
        public EstadoPedido Estado { get; set; }
        public int? PromocionId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
