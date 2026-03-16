using EmpanadasProject.Data.Entities.Productos;
using System;

namespace EmpanadasProject.Data.Entities
{
    public class ProductoEnNegocio : ProductoBase
    {
        public int NegocioId { get; set; }
    }
}