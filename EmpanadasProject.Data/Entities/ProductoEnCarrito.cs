using EmpanadasProject.Data.Entities.Productos;
using System;

namespace EmpanadasProject.Data.Entities
{
    public class ProductoEnCarrito : ProductoBase
    {
        public int ProductoEnNegocioId { get; set; }
        public int CarritoId { get; set; }
    }
}
