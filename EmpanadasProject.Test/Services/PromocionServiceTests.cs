using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Services;
using EmpanadasProject.Test.BaseContext;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmpanadasProject.Test.Services
{
    public class PromocionServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddPromocion()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new PromocionService(context);
            var promocion = new Promocion { Nombre = "Promo Verano", Descuento = 15.5f, Fecha = DateTime.UtcNow };

            // Act
            var result = await service.AddAsync(promocion);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("Promo Verano", result.Data.Nombre);
            Assert.Equal(1, context.Promociones.Count());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPromociones()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new PromocionService(context);
            context.Promociones.Add(new Promocion { Nombre = "Promo 1", Descuento = 10, Fecha = DateTime.UtcNow });
            context.Promociones.Add(new Promocion { Nombre = "Promo 2", Descuento = 20, Fecha = DateTime.UtcNow });
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPromocion_WhenExists()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new PromocionService(context);
            var promo = new Promocion { Nombre = "Promo 1", Descuento = 10, Fecha = DateTime.UtcNow };
            context.Promociones.Add(promo);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetByIdAsync(promo.Id);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(promo.Id, result.Data.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePromocion_WhenExists()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new PromocionService(context);
            var promo = new Promocion { Nombre = "Promo Original", Descuento = 10, Fecha = DateTime.UtcNow };
            context.Promociones.Add(promo);
            await context.SaveChangesAsync();

            // Act
            promo.Nombre = "Promo Actualizada";
            promo.Descuento = 25;
            var result = await service.UpdateAsync(promo);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Promo Actualizada", result.Data.Nombre);
            Assert.Equal(25, result.Data.Descuento);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePromocion_WhenExists()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new PromocionService(context);
            var promo = new Promocion { Nombre = "Promo a Eliminar", Descuento = 10, Fecha = DateTime.UtcNow };
            context.Promociones.Add(promo);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteAsync(promo.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Empty(context.Promociones);
        }
    }
}
