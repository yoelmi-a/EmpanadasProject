using EmpanadasProject.Data.Entities.Usuario;
using FluentValidation;

namespace EmpanadasProject.Web.Validations.Usuario
{
    public class RolUsuarioValidator : AbstractValidator<RolUsuario>
    {
        public RolUsuarioValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio")
                .MinimumLength(3).WithMessage("El nombre del rol debe tener al menos 3 caracteres")
                .MaximumLength(50).WithMessage("El nombre del rol no puede exceder 50 caracteres");
        }
    }
}
