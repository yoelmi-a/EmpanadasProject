using System;

namespace EmpanadasProject.Data.Entities
{
    /// <summary>
    /// Registra el historial de cambios de estado de un pedido.
    /// </summary>
    public class HistorialEstado : EntidadBase
    {
        public int PedidoId { get; set; }
        public EstadoPedido Estado { get; set; }
        public DateTime FechaCambio { get; set; } = DateTime.UtcNow;
        public string? Observaciones { get; set; }
    }
}
