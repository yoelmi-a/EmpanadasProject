using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Validations;
using FluentValidation.TestHelper;
using Xunit;

namespace EmpanadasProject.Test.Validations
{
    public class ProductoEnCarritoValidatorTests
    {
        private readonly ProductoEnCarritoValidator _validator;

        public ProductoEnCarritoValidatorTests()
        {
            _validator = new ProductoEnCarritoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_CarritoId_Is_Zero_Or_Less()
        {
            var model = new ProductoEnCarrito { CarritoId = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CarritoId)
                  .WithErrorMessage("El ID del carrito es requerido y debe ser mayor a 0.");
        }

        [Fact]
        public void Should_Have_Error_When_Cantidad_Is_Zero_Or_Less()
        {
            var model = new ProductoEnCarrito { Cantidad = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Cantidad)
                  .WithErrorMessage("La cantidad de productos debe ser mayor a 0.");
        }

        [Fact]
        public void Should_Have_Error_When_Precio_Is_Zero_Or_Less()
        {
            var model = new ProductoEnCarrito { Precio = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Precio)
                  .WithErrorMessage("El precio del producto debe ser mayor a 0.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_ProductoEnCarrito_Is_Valid()
        {
            var model = new ProductoEnCarrito 
            { 
                CarritoId = 1, 
                Cantidad = 2, 
                Precio = 15.5m 
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
