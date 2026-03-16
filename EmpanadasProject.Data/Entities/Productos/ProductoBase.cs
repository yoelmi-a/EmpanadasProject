using EmpanadasProject.Data.Entities.BaseEntity;

namespace EmpanadasProject.Data.Entities.Productos
{
    public abstract class ProductoBase : BaseEntity.BaseEntity
    {
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
