using EmpanadasProject.Data.Entities.Usuario;
using FluentValidation;

namespace EmpanadasProject.Web.Validations.Usuario
{
    public class MetodoPagoValidator : AbstractValidator<MetodoPago>
    {
        public MetodoPagoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del método de pago es obligatorio")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres")
                .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");
        }
    }
}
