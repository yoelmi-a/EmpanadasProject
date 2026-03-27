using EmpanadasProject.Data.Entities.Productos;
using FluentValidation;

namespace EmpanadasProject.Web.Validations.Productos
{
    public class ProductoValidator : AbstractValidator<Producto>
    {
        public ProductoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Nombre).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Descripcion).NotEmpty();
            RuleFor(x => x.Precio).GreaterThan(0);
            RuleFor(x => x.Cantidad).GreaterThanOrEqualTo(0);
        }
    }
}
