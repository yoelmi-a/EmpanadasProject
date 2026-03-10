using EmpanadasProject.Data.Entities;
using FluentValidation;

namespace EmpanadasProject.Data.Validations
{
    public class PromocionValidator : AbstractValidator<Promocion>
    {
        public PromocionValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la promoción es requerido.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            RuleFor(x => x.Descuento)
                .GreaterThan(0).WithMessage("El descuento debe ser mayor a 0.")
                .LessThanOrEqualTo(100).WithMessage("El descuento no puede ser mayor al 100%.");

            RuleFor(x => x.Fecha)
                .NotEmpty().WithMessage("La fecha de la promoción es requerida.");
        }
    }
}
