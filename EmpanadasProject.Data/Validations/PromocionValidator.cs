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
                .GreaterThan(0).WithMessage("El descuento debe ser mayor a 0.");

            RuleFor(x => x.FechaInicio)
                .NotEmpty().WithMessage("La fecha de inicio es requerida.");

            RuleFor(x => x.FechaFin)
                .NotEmpty().WithMessage("La fecha de fin es requerida.")
                .GreaterThan(x => x.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");
        }
    }
}
