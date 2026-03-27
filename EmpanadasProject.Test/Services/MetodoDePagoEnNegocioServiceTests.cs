using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Services;
using EmpanadasProject.Test.BaseContext;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmpanadasProject.Test.Services
{
    public class MetodoDePagoEnNegocioServiceTests
    {
        [Fact]
        public async Task AgregarMetodoPagoAsync_ShouldAddMetodoPago_WhenNotExists()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new MetodoDePagoEnNegocioService(context);
            var metodo = new MetodoDePagoEnNegocio { MetodoId = "M1", NegocioId = "N1" };

            // Act
            var result = await service.AgregarMetodoPagoAsync(metodo);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, context.MetodosDePagoEnNegocio.Count());
        }

        [Fact]
        public async Task AgregarMetodoPagoAsync_ShouldFail_WhenAlreadyExists()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new MetodoDePagoEnNegocioService(context);
            var metodo = new MetodoDePagoEnNegocio { MetodoId = "M1", NegocioId = "N1" };
            context.MetodosDePagoEnNegocio.Add(metodo);
            await context.SaveChangesAsync();

            // Act
            var result = await service.AgregarMetodoPagoAsync(new MetodoDePagoEnNegocio { MetodoId = "M1", NegocioId = "N1" });

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El método de pago ya existe en este negocio.", result.Message);
        }

        [Fact]
        public async Task RemoverMetodoPagoAsync_ShouldRemoveMetodoPago_WhenExists()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new MetodoDePagoEnNegocioService(context);
            var metodo = new MetodoDePagoEnNegocio { MetodoId = "M1", NegocioId = "N1" };
            context.MetodosDePagoEnNegocio.Add(metodo);
            await context.SaveChangesAsync();

            // Act
            var result = await service.RemoverMetodoPagoAsync("M1", "N1");

            // Assert
            Assert.True(result.Success);
            Assert.Empty(context.MetodosDePagoEnNegocio);
        }

        [Fact]
        public async Task RemoverMetodoPagoAsync_ShouldFail_WhenNotExists()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new MetodoDePagoEnNegocioService(context);

            // Act
            var result = await service.RemoverMetodoPagoAsync("M1", "N1");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El método de pago no se encuentra en este negocio.", result.Message);
        }
        [Fact]
        public async Task ObtenerPorNegocioAsync_ShouldReturnMethodsForGivenBusiness()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new MetodoDePagoEnNegocioService(context);
            context.MetodosDePagoEnNegocio.Add(new MetodoDePagoEnNegocio { MetodoId = "M1", NegocioId = "N1" });
            context.MetodosDePagoEnNegocio.Add(new MetodoDePagoEnNegocio { MetodoId = "M2", NegocioId = "N1" });
            context.MetodosDePagoEnNegocio.Add(new MetodoDePagoEnNegocio { MetodoId = "M1", NegocioId = "N2" });
            await context.SaveChangesAsync();

            // Act
            var result = await service.ObtenerPorNegocioAsync("N1");

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count());
            Assert.All(result.Data, item => Assert.Equal("N1", item.NegocioId));
        }
    }
}
