using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Productos;
using EmpanadasProject.Data.Services;
using EmpanadasProject.Test.BaseContext;
using EmpanadasProject.Web.Validations.Productos;
using FluentValidation.Results;

namespace EmpanadasProject.Test.Productos
{
    public class ProductoServiceTest
    {
        private ProductoService CrearServicio(EmpanadasContext context)
        {
            return new ProductoService(context);
        }

        private ProductoValidator CrearValidator()
        {
            return new ProductoValidator();
        }

        [Fact]
        public async Task AddProductoAsync_ProductoValido_DebeGuardarse()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);
            var validator = CrearValidator();

            var producto = new Producto
            {
                Id = 1,
                Nombre = "Empanada de Pollo",
                Descripcion = "Deliciosa empanada de pollo",
                Precio = 50,
                Cantidad = 100
            };

            ValidationResult validation = await validator.ValidateAsync(producto);
            Assert.True(validation.IsValid);

            var result = await service.AddProductoAsync(producto);
            Assert.True(result.Success);
            Assert.Equal(1, context.Productos.Count());
        }

        [Fact]
        public async Task AddProductoAsync_PrecioInvalido_DebeFallarValidacion()
        {
            var validator = CrearValidator();

            var producto = new Producto
            {
                Id = 1,
                Nombre = "Empanada de Pollo",
                Descripcion = "Deliciosa empanada de pollo",
                Precio = 0,
                Cantidad = 100
            };

            ValidationResult validation = await validator.ValidateAsync(producto);
            Assert.False(validation.IsValid);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Precio");
        }

        [Fact]
        public async Task GetProductoByIdAsync_DebeRetornarProducto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);

            context.Productos.Add(new Producto { Id = 1, Nombre = "Producto 1", Descripcion = "Desc", Precio = 10, Cantidad = 5 });
            await context.SaveChangesAsync();

            var result = await service.GetProductoByIdAsync(1);
            Assert.NotNull(result.Data);
            Assert.Equal("Producto 1", result.Data.Nombre);
        }

        [Fact]
        public async Task UpdateProductoAsync_DebeActualizarNombre()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);

            context.Productos.Add(new Producto { Id = 1, Nombre = "Original", Descripcion = "Desc", Precio = 10, Cantidad = 5 });
            await context.SaveChangesAsync();

            var producto = await context.Productos.FindAsync(1);
            producto.Nombre = "Actualizado";

            await service.UpdateProductoAsync(producto);
            var actualizado = await context.Productos.FindAsync(1);
            Assert.Equal("Actualizado", actualizado.Nombre);
        }

        [Fact]
        public async Task DeleteProductoAsync_DebeEliminar()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();
            var service = CrearServicio(context);

            context.Productos.Add(new Producto { Id = 1, Nombre = "Borrar", Descripcion = "Desc", Precio = 10, Cantidad = 5 });
            await context.SaveChangesAsync();

            await service.DeleteProductoAsync(1);
            var producto = await context.Productos.FindAsync(1);
            Assert.Null(producto);
        }
    }
}
