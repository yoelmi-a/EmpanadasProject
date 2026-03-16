using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Validations;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace EmpanadasProject.Test.Validations
{
    public class PromocionValidatorTests
    {
        private readonly PromocionValidator _validator;

        public PromocionValidatorTests()
        {
            _validator = new PromocionValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Nombre_Is_Empty()
        {
            var model = new Promocion { Nombre = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Nombre)
                  .WithErrorMessage("El nombre de la promoción es requerido.");
        }

        [Fact]
        public void Should_Have_Error_When_Nombre_Exceeds_100_Characters()
        {
            var model = new Promocion { Nombre = new string('A', 101) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Nombre)
                  .WithErrorMessage("El nombre no puede exceder los 100 caracteres.");
        }

        [Fact]
        public void Should_Have_Error_When_Descuento_Is_Zero_Or_Less()
        {
            var model = new Promocion { Descuento = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Descuento)
                  .WithErrorMessage("El descuento debe ser mayor a 0.");

            var modelNegative = new Promocion { Descuento = -5 };
            var resultNegative = _validator.TestValidate(modelNegative);
            resultNegative.ShouldHaveValidationErrorFor(x => x.Descuento)
                          .WithErrorMessage("El descuento debe ser mayor a 0.");
        }

        [Fact]
        public void Should_Have_Error_When_Descuento_Is_Greater_Than_100()
        {
            var model = new Promocion { Descuento = 101 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Descuento)
                  .WithErrorMessage("El descuento no puede ser mayor al 100%.");
        }

        [Fact]
        public void Should_Have_Error_When_Fecha_Is_Empty()
        {
            var model = new Promocion { Fecha = default };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Fecha)
                  .WithErrorMessage("La fecha de la promoción es requerida.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Promocion_Is_Valid()
        {
            var model = new Promocion 
            { 
                Nombre = "Promo Valida", 
                Descuento = 15, 
                Fecha = DateTime.UtcNow 
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
