using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Pedidos;
using EmpanadasProject.Data.Services;
using EmpanadasProject.Test.BaseContext;
using EmpanadasProject.Web.Validations.Pedidos;
using FluentValidation.Results;

namespace EmpanadasProject.Test.Pedidos
{
    public class PedidoServiceTest
    {
        private PedidoService CrearServicio(EmpanadasContext context)
        {
            return new PedidoService(context);
        }

        private PedidoValidator CrearValidator()
        {
            return new PedidoValidator();
        }

        [Fact]
        public async Task AddPedidoAsync_PedidoValido_DebeGuardarse()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);
            var validator = CrearValidator();

            var pedido = new Pedido
            {
                Id = 1,
                ClienteId = 1,
                NegocioId = 1,
                MetodoDePagoId = 1,
                TotalPagado = 150,
                Estado = EstadoPedido.Proceso,
                Fecha = DateTime.Now
            };

            ValidationResult validation = await validator.ValidateAsync(pedido);
            Assert.True(validation.IsValid);

            var result = await service.AddPedidoAsync(pedido);
            Assert.True(result.Success);
            Assert.Equal(1, context.Pedidos.Count());
        }

        [Fact]
        public async Task AddPedidoAsync_ClienteIdInvalido_DebeFallarValidacion()
        {
            var validator = CrearValidator();

            var pedido = new Pedido
            {
                Id = 1,
                ClienteId = 0,
                NegocioId = 1,
                MetodoDePagoId = 1,
                TotalPagado = 150,
                Estado = EstadoPedido.Proceso,
                Fecha = DateTime.Now
            };

            ValidationResult validation = await validator.ValidateAsync(pedido);
            Assert.False(validation.IsValid);
            Assert.Contains(validation.Errors, e => e.PropertyName == "ClienteId");
        }

        [Fact]
        public async Task GetPedidoByIdAsync_DebeRetornarPedido()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);

            context.Pedidos.Add(new Pedido { Id = 1, ClienteId = 1, NegocioId = 1, MetodoDePagoId = 1, Fecha = DateTime.Now });
            await context.SaveChangesAsync();

            var result = await service.GetPedidoByIdAsync(1);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
        }

        [Fact]
        public async Task UpdatePedidoAsync_DebeCambiarEstado()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);

            context.Pedidos.Add(new Pedido { Id = 1, Estado = EstadoPedido.Proceso, Fecha = DateTime.Now });
            await context.SaveChangesAsync();

            var pedido = await context.Pedidos.FindAsync(1);
            pedido.Estado = EstadoPedido.Aceptado;

            await service.UpdatePedidoAsync(pedido);
            var actualizado = await context.Pedidos.FindAsync(1);
            Assert.Equal(EstadoPedido.Aceptado, actualizado.Estado);
        }
    }
}
