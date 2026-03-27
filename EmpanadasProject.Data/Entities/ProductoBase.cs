using System;

namespace EmpanadasProject.Data.Entities
{
    public class ProductoBase : BaseEntity.BaseEntity
    {
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
