using System;

namespace EmpanadasProject.Data.Entities
{
    public class Promocion : BaseEntity.BaseEntity
    {
        public string Nombre { get; set; }
        public float Descuento { get; set; }
        public DateTime Fecha { get; set; }
    }
}
