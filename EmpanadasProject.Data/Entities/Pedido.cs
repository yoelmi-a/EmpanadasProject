using System;
using System.Collections.Generic;

namespace EmpanadasProject.Data.Entities
{
    /// <summary>
    /// Representa un pedido realizado por un cliente.
    /// </summary>
    public class Pedido : EntidadBase
    {
        public string ClienteId { get; set; } = string.Empty;
        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        public EstadoPedido Estado { get; set; } = EstadoPedido.Recibido;
        public string MetodoEntrega { get; set; } = string.Empty; // Domicilio o Recogida
        
        // Simulación de pago
        public string ReferenciaPago { get; set; } = string.Empty;
        
        public List<ItemPedido> Items { get; set; } = [];
        public List<HistorialEstado> Historiales { get; set; } = [];
    }
}
