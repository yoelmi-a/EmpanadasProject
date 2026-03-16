using EmpanadasProject.Data.Entities.Pedidos;
using FluentValidation;

namespace EmpanadasProject.Web.Validations.Pedidos
{
    public class ProductoEnPedidoValidator : AbstractValidator<ProductoEnPedido>
    {
        public ProductoEnPedidoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.PedidoId).GreaterThan(0);
            RuleFor(x => x.Precio).GreaterThan(0);
            RuleFor(x => x.Cantidad).GreaterThan(0);
        }
    }
}
