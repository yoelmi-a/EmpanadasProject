using EmpanadasProject.Data.Entities;
using FluentValidation;

namespace EmpanadasProject.Data.Validations
{
    public class MetodoDePagoEnNegocioValidator : AbstractValidator<MetodoDePagoEnNegocio>
    {
        public MetodoDePagoEnNegocioValidator()
        {
            RuleFor(x => x.MetodoId)
                .NotEmpty().WithMessage("El ID del método de pago es requerido.");

            RuleFor(x => x.NegocioId)
                .NotEmpty().WithMessage("El ID del negocio es requerido.");
        }
    }
}
