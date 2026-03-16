using EmpanadasProject.Data.Entities;
using FluentValidation;

namespace EmpanadasProject.Data.Validations
{
    public class ProductoEnCarritoValidator : AbstractValidator<ProductoEnCarrito>
    {
        public ProductoEnCarritoValidator()
        {
            RuleFor(x => x.CarritoId)
                .GreaterThan(0).WithMessage("El ID del carrito es requerido y debe ser mayor a 0.");

            RuleFor(x => x.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad de productos debe ser mayor a 0.");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio del producto debe ser mayor a 0.");
        }
    }
}
