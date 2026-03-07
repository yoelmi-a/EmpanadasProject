using EmpanadasProject.Data.Entities.Usuario;
using FluentValidation;

namespace EmpanadasProject.Web.Validations.Usuario
{
    public class UsuarioValidator : AbstractValidator<Usuarios>
    {
        public UsuarioValidator()
        {

            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Contraseña)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.Telefono)
                .NotEmpty();

            RuleFor(x => x.RolId)
                .GreaterThan(0);

            RuleFor(x => x.FechaDeRegistro)
                .LessThanOrEqualTo(DateTime.Now);
        }
    }
}
