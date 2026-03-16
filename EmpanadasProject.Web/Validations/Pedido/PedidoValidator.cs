using EmpanadasProject.Data.Entities.Pedidos;
using FluentValidation;

namespace EmpanadasProject.Web.Validations.Pedidos
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.NegocioId).GreaterThan(0);
            RuleFor(x => x.MetodoDePagoId).GreaterThan(0);
            RuleFor(x => x.TotalPagado).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Estado).IsInEnum();
            RuleFor(x => x.Fecha).Must(f => f != default);
        }
    }
}
