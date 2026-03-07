

using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Repositories;
using EmpanadasProject.Test.BaseContext;
using EmpanadasProject.Web.Validations.Usuario;
using FluentValidation.Results;

namespace EmpanadasProject.Test.Usuario
{
    public class UsuarioServiceTest
    {
        private UsuarioServiece CrearServicio(EmpanadasContext context)
        {
            return new UsuarioServiece(context);
        }
        private UsuarioValidator CrearValidator()
        {
            return new UsuarioValidator();
        }

        [Fact]
        public async Task AddUsuarioAsync_UsuarioValido_DebeGuardarse()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var usuario = new Usuarios
            {
                Id = 1,
                Nombre = "Juan",
                Email = "juan@email.com",
                Contraseña = "123456789",
                Telefono = "8090000000",
                FechaDeRegistro = DateTime.Now,
                RolId = 1
            };

            ValidationResult validation = validator.Validate(usuario);

            Assert.True(validation.IsValid);

            var result = await service.AddUsuarioAsync(usuario);

            Assert.True(result.Success);
            Assert.Equal(1, context.Usuarios.Count());
        }

        [Fact]
        public async Task AddUsuarioAsync_NombreVacio_DebeFallarValidacion()
        {
            var validator = CrearValidator();

            var usuario = new Usuarios
            {
                Id = 2,
                Nombre = "",
                Email = "test@email.com",
                Contraseña = "123456",
                Telefono = "8090000000",
                FechaDeRegistro = DateTime.Now,
                RolId = 1
            };

            ValidationResult validation = await validator.ValidateAsync(usuario);

            Assert.False(validation.IsValid);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Nombre");
        }

        [Fact]
        public async Task AddUsuarioAsync_EmailInvalido_DebeFallarValidacion()
        {
            var validator = CrearValidator();

            var usuario = new Usuarios
            {
                Id = 3,
                Nombre = "Pedro",
                Email = "correo_invalido",
                Contraseña = "123456",
                Telefono = "8090000000",
                FechaDeRegistro = DateTime.Now,
                RolId = 1
            };

            ValidationResult validation = await validator.ValidateAsync(usuario);

            Assert.False(validation.IsValid);
            Assert.Contains(validation.Errors, e => e.PropertyName == "Email");
        }

        [Fact]
        public async Task AddUsuarioAsync_RolIdInvalido_DebeFallarValidacion()
        {
            var validator = CrearValidator();

            var usuario = new Usuarios
            {
                Id = 4,
                Nombre = "Luis",
                Email = "luis@email.com",
                Contraseña = "123456",
                Telefono = "8090000000",
                FechaDeRegistro = DateTime.Now,
                RolId = 0
            };

            var validation =  await validator.ValidateAsync(usuario);

            Assert.False(validation.IsValid);
            Assert.Contains(validation.Errors, e => e.PropertyName == "RolId");
        }

        [Fact]
        public async Task AddUsuarioAsync_DebeAsignarFechaRegistro()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var usuario = new Usuarios
            {
                Id = 5,
                Nombre = "Maria",
                Email = "maria@email.com",
                Contraseña = "123456789",
                Telefono = "8090000000",
                RolId = 1
            };

            var validation = await validator.ValidateAsync(usuario);

            Assert.False(validation.IsValid);

            var result = await service.AddUsuarioAsync(usuario);

            Assert.True(result.Data.FechaDeRegistro != default);
        }
    }
}
