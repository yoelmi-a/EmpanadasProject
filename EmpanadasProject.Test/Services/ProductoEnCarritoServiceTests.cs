using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Services;
using EmpanadasProject.Test.BaseContext;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmpanadasProject.Test.Services
{
    public class ProductoEnCarritoServiceTests
    {
        [Fact]
        public async Task AgregarAsync_ShouldAddProductoToCarrito()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new ProductoEnCarritoService(context);
            var producto = new ProductoEnCarrito { CarritoId = 1, Cantidad = 2, Precio = 15.0m };

            // Act
            var result = await service.AgregarAsync(producto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, context.ProductosEnCarrito.Count());
        }

        [Fact]
        public async Task LimpiarCarritoAsync_ShouldRemoveAllProductosForGivenCarrito()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new ProductoEnCarritoService(context);
            
            context.ProductosEnCarrito.Add(new ProductoEnCarrito { CarritoId = 1, Cantidad = 1, Precio = 10.0m });
            context.ProductosEnCarrito.Add(new ProductoEnCarrito { CarritoId = 1, Cantidad = 1, Precio = 20.0m });
            context.ProductosEnCarrito.Add(new ProductoEnCarrito { CarritoId = 2, Cantidad = 1, Precio = 5.0m });
            await context.SaveChangesAsync();

            // Act
            var result = await service.LimpiarCarritoAsync(1);

            // Assert
            Assert.True(result.Success);
            var remainingProducts = context.ProductosEnCarrito.ToList();
            Assert.Single(remainingProducts);
            Assert.Equal(2, remainingProducts.First().CarritoId);
        }

        [Fact]
        public async Task RemoverAsync_ShouldRemoveSpecificProducto()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new ProductoEnCarritoService(context);
            var producto = new ProductoEnCarrito { CarritoId = 1, Cantidad = 1, Precio = 10.0m };
            context.ProductosEnCarrito.Add(producto);
            await context.SaveChangesAsync();

            // Act
            var result = await service.RemoverAsync(producto.Id);

            // Assert
            Assert.True(result.Success);
            Assert.Empty(context.ProductosEnCarrito);
        }
        [Fact]
        public async Task ObtenerPorCarritoAsync_ShouldReturnProductsForGivenCart()
        {
            // Arrange
            using var context = TestDbContextFactory.Create();
            var service = new ProductoEnCarritoService(context);
            context.ProductosEnCarrito.Add(new ProductoEnCarrito { CarritoId = 1, Cantidad = 1, Precio = 10.0m });
            context.ProductosEnCarrito.Add(new ProductoEnCarrito { CarritoId = 1, Cantidad = 2, Precio = 20.0m });
            context.ProductosEnCarrito.Add(new ProductoEnCarrito { CarritoId = 2, Cantidad = 3, Precio = 30.0m });
            await context.SaveChangesAsync();

            // Act
            var result = await service.ObtenerPorCarritoAsync(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count());
            Assert.All(result.Data, item => Assert.Equal(1, item.CarritoId));
        }
    }
}
