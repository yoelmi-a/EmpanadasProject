using System;

namespace EmpanadasProject.Data.Entities
{
    /// <summary>
    /// Entidad base con campos de auditoría.
    /// </summary>
    public abstract class EntidadBase
    {
        public int Id { get; set; }
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
        public DateTime? ActualizadoEn { get; set; }
    }
}
