using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Pedidos;
using EmpanadasProject.Data.Services;
using EmpanadasProject.Test.BaseContext;
using EmpanadasProject.Web.Validations.Pedidos;
using FluentValidation.Results;

namespace EmpanadasProject.Test.Pedidos
{
    public class ProductoEnPedidoServiceTest
    {
        private ProductoEnPedidoService CrearServicio(EmpanadasContext context)
        {
            return new ProductoEnPedidoService(context);
        }

        private ProductoEnPedidoValidator CrearValidator()
        {
            return new ProductoEnPedidoValidator();
        }

        [Fact]
        public async Task AddProductoEnPedidoAsync_Valido_DebeGuardarse()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);
            var validator = CrearValidator();

            var productoEnPedido = new ProductoEnPedido
            {
                Id = 1,
                PedidoId = 1,
                Precio = 50,
                Cantidad = 2
            };

            ValidationResult validation = await validator.ValidateAsync(productoEnPedido);
            Assert.True(validation.IsValid);

            var result = await service.AddProductoEnPedidoAsync(productoEnPedido);
            Assert.True(result.Success);
            Assert.Equal(1, context.ProductoEnPedidos.Count());
        }

        [Fact]
        public async Task GetProductosByPedidoIdAsync_DebeRetornarLista()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);

            context.ProductoEnPedidos.Add(new ProductoEnPedido { Id = 1, PedidoId = 10, Precio = 50, Cantidad = 2 });
            context.ProductoEnPedidos.Add(new ProductoEnPedido { Id = 2, PedidoId = 10, Precio = 40, Cantidad = 1 });
            await context.SaveChangesAsync();

            var result = await service.GetProductosByPedidoIdAsync(10);
            Assert.Equal(2, result.Data.Count());
        }
    }
}
