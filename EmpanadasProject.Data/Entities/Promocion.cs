using System;

namespace EmpanadasProject.Data.Entities
{
    /// <summary>
    /// Representa una promoción aplicable a un pedido.
    /// </summary>
    public class Promocion : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Descuento { get; set; }
        public string TipoDescuento { get; set; } = "Porcentaje"; // Porcentaje o Fijo
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool EstaActiva { get; set; } = true;
    }
}
