using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Services;
using EmpanadasProject.Test.BaseContext;
using EmpanadasProject.Web.Validations.Usuario;

namespace EmpanadasProject.Test.Usuario
{
    public class MetodoPagoServiceTest
    {
        private MetodoPagoService CrearServicio(EmpanadasContext context)
        {
            return new MetodoPagoService(context);
        }
        private MetodoPagoValidator CrearValidator()
        {
            return new MetodoPagoValidator();
        }

        #region AddMetodoPagoAsync
        [Fact]
        public async Task AddMetodoPagoAsync_MetodoPagoValido_DebeAgregar()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var metodo = new MetodoPago
            {
                Id = 1,
                Nombre = "Efectivo"
            };

            var validation = await validator.ValidateAsync(metodo);

            Assert.True(validation.IsValid);

            var result = await service.AddMetodoPagoAsync(metodo);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task MetodoPagoValidator_IdInvalido()
        {
            var validator = CrearValidator();

            var metodo = new MetodoPago
            {
                Id = 0,
                Nombre = "Tarjeta"
            };

            var result = await validator.ValidateAsync(metodo);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task MetodoPagoValidator_NombreVacio()
        {
            var validator = CrearValidator();

            var metodo = new MetodoPago
            {
                Id = 1,
                Nombre = ""
            };

            var result = await validator.ValidateAsync(metodo);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task MetodoPagoValidator_NombreMuyCorto()
        {
            var validator = CrearValidator();

            var metodo = new MetodoPago
            {
                Id = 1,
                Nombre = "AB"
            };

            var result = await validator.ValidateAsync(metodo);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task MetodoPagoValidator_NombreMuyLargo()
        {
            var validator = CrearValidator();

            var metodo = new MetodoPago
            {
                Id = 1,
                Nombre = new string('A', 60)
            };

            var result = await validator.ValidateAsync(metodo);

            Assert.False(result.IsValid);
        }
        #endregion

        #region DeleteMetodoPagoAsync
        [Fact]
        public async Task DeleteMetodoPago_MetodoExiste_DebeEliminar()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            context.MetodoPagos.Add(new MetodoPago { Id = 1, Nombre = "Efectivo" });
            await context.SaveChangesAsync();

            var result = await service.DeleteMetodoPago(1);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteMetodoPago_DebeRemoverRegistro()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            context.MetodoPagos.Add(new MetodoPago { Id = 2, Nombre = "Tarjeta" });
            await context.SaveChangesAsync();


            await service.DeleteMetodoPago(2);

            var metodo = context.MetodoPagos.FirstOrDefault(m => m.Id == 2);

            Assert.Null(metodo);
        }

        [Fact]
        public async Task DeleteMetodoPago_MetodoNoExiste()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.DeleteMetodoPago(100);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteMetodoPago_DataDebeSerNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.DeleteMetodoPago(10);

            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteMetodoPago_MensajeCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.MetodoPagos.Add(new MetodoPago { Id = 3, Nombre = "Paypal" });
            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.DeleteMetodoPago(3);

            Assert.Equal("Metodo de Pago eliminado exitosamente.", result.Message);
        }
        #endregion

        #region GetMetodoPagoByIdAsync
        [Fact]
        public async Task GetMetodoPagoByIdAsync_MetodoExiste()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            context.MetodoPagos.Add(new MetodoPago { Id = 1, Nombre = "Efectivo" });
            await context.SaveChangesAsync();


            var result = await service.GetMetodoPagoByIdAsync(1);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetMetodoPagoByIdAsync_NombreCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            context.MetodoPagos.Add(new MetodoPago { Id = 2, Nombre = "Tarjeta" });
            await context.SaveChangesAsync();


            var result = await service.GetMetodoPagoByIdAsync(2);

            Assert.Equal("Tarjeta", result.Data.Nombre);
        }

        [Fact]
        public async Task GetMetodoPagoByIdAsync_NoExiste()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetMetodoPagoByIdAsync(99);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetMetodoPagoByIdAsync_DataNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetMetodoPagoByIdAsync(50);

            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetMetodoPagoByIdAsync_MensajeCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.MetodoPagos.Add(new MetodoPago { Id = 3, Nombre = "Transferencia" });
            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.GetMetodoPagoByIdAsync(3);

            Assert.Equal("Metodo de Pago obtenido exitosamente.", result.Message);
        }
        #endregion

        #region GetAllMetodoPagosAsync  
        [Fact]
        public async Task GetAllMetodoPago_DebeRetornarLista()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetAllMetodoPago();

            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetAllMetodoPago_DebeRetornarRegistros()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            context.MetodoPagos.Add(new MetodoPago { Id = 1, Nombre = "Efectivo" });
            await context.SaveChangesAsync();

            var result = await service.GetAllMetodoPago();

            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetAllMetodoPago_SinRegistros()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetAllMetodoPago();

            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetAllMetodoPago_SuccessDebeSerTrue()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetAllMetodoPago();

            Assert.True(result.Success);
        }


        [Fact]
        public async Task GetAllMetodoPago_MensajeCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetAllMetodoPago();

            Assert.Equal("Metodos de Pago obtenidos exitosamente.", result.Message);
        }
        #endregion

        #region UpdateMetodoPagoAsync
        [Fact]
        public async Task UpdateMetodoPago_DebeActualizarMetodo()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            context.MetodoPagos.Add(new MetodoPago { Id = 1, Nombre = "Efectivo" });
            await context.SaveChangesAsync();

            var metodo = new MetodoPago { Id = 1, Nombre = "Tarjeta" };

            var result = await service.UpdateMetodoPago(metodo);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateMetodoPago_CambiaNombre()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            context.MetodoPagos.Add(new MetodoPago { Id = 2, Nombre = "Efectivo" });
            await context.SaveChangesAsync();

            await service.UpdateMetodoPago(new MetodoPago { Id = 2, Nombre = "Transferencia" });

            var metodo = context.MetodoPagos.First();

            Assert.Equal("Transferencia", metodo.Nombre);
        }

        [Fact]
        public async Task UpdateMetodoPago_NoExiste()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.UpdateMetodoPago(new MetodoPago { Id = 99, Nombre = "Test" });

            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateMetodoPago_DataNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.UpdateMetodoPago(new MetodoPago { Id = 20, Nombre = "Test" });

            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateMetodoPago_MensajeCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.MetodoPagos.Add(new MetodoPago { Id = 4, Nombre = "Efectivo" });
            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.UpdateMetodoPago(new MetodoPago { Id = 4, Nombre = "Tarjeta" });

            Assert.Equal("Metodo de Pago actualizado exitosamente.", result.Message);
        }
        #endregion

    }
}