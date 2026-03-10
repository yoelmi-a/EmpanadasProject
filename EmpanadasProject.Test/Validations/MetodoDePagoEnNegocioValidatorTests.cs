using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Validations;
using FluentValidation.TestHelper;
using Xunit;

namespace EmpanadasProject.Test.Validations
{
    public class MetodoDePagoEnNegocioValidatorTests
    {
        private readonly MetodoDePagoEnNegocioValidator _validator;

        public MetodoDePagoEnNegocioValidatorTests()
        {
            _validator = new MetodoDePagoEnNegocioValidator();
        }

        [Fact]
        public void Should_Have_Error_When_MetodoId_Is_Empty()
        {
            var model = new MetodoDePagoEnNegocio { MetodoId = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.MetodoId)
                  .WithErrorMessage("El ID del método de pago es requerido.");
        }

        [Fact]
        public void Should_Have_Error_When_NegocioId_Is_Empty()
        {
            var model = new MetodoDePagoEnNegocio { NegocioId = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.NegocioId)
                  .WithErrorMessage("El ID del negocio es requerido.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_MetodoDePagoEnNegocio_Is_Valid()
        {
            var model = new MetodoDePagoEnNegocio 
            { 
                MetodoId = "M1", 
                NegocioId = "N1" 
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
