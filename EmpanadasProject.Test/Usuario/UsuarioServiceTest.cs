

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

        #region Pruebas para AddUsuarioAsync

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

            var validation = await validator.ValidateAsync(usuario);

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

            var result = await service.AddUsuarioAsync(usuario);

            Assert.NotEqual(default, result.Data.FechaDeRegistro);
        }
        #endregion

        #region Pruebas para GetUsuarioByIdAsync
        [Fact]
        public async Task GetUsuarioByIdAsync_DebeRetornarUsuario()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 1,
                Nombre = "Juan",
                Email = "juan@email.com",
                Contraseña = "12345678",
                Telefono = "8090000001",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var result = await service.GetUsuarioByIdAsync(1);

            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_DebeRetornarNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var result = await service.GetUsuarioByIdAsync(50);

            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_DebeRetornarIdCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 3,
                Nombre = "Pedro",
                Email = "pedro@email.com",
                Contraseña = "12345678",
                Telefono = "8090000002",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var result = await service.GetUsuarioByIdAsync(3);

            Assert.Equal(3, result.Data.Id);
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_DebeRetornarEmailCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 2,
                Nombre = "Ana",
                Email = "ana@email.com",
                Contraseña = "12345678",
                Telefono = "8090000005",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var result = await service.GetUsuarioByIdAsync(2);

            Assert.Equal("ana@email.com", result.Data.Email);
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_NoDebeLanzarError()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var result = await service.GetUsuarioByIdAsync(100);

            Assert.True(true);
        }
        #endregion

        #region Pruebas para GetUsuariosByEmailAsync
        [Fact]
        public async Task GetUsuariosByEmailAsync_DebeRetornarTrue()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Email = "test@email.com",
                Nombre = "Test",
                Contraseña = "12345678",
                Telefono = "809",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var result = await service.GetUsuariosByEmailAsync("test@email.com");

            Assert.True(result.Success);
        }

        [Fact]
        public async Task ExisteEmailAsync_DebeRetornarFalse()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var result = await service.GetUsuariosByEmailAsync("noexiste@email.com");

            Assert.False(result.Success);
        }

        public async Task GetUsuariosByEmailAsync_EmailDuplicadoDebeRetornarTrue()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios { Email = "dup@email.com", Nombre = "A", Contraseña = "12345678", Telefono = "809", RolId = 1 });
            context.Usuarios.Add(new Usuarios { Email = "dup@email.com", Nombre = "B", Contraseña = "12345678", Telefono = "809", RolId = 1 });

            await context.SaveChangesAsync();

            var result = await service.GetUsuariosByEmailAsync("dup@email.com");

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetUsuariosByEmailAsync_EmailNullDebeRetornarFalse()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var result = await service.GetUsuariosByEmailAsync(null);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetUsuariosByEmailAsync_EmailMayusculas()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Email = "correo@email.com",
                Nombre = "Test",
                Contraseña = "12345678",
                Telefono = "809",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var result = await service.GetUsuariosByEmailAsync("CORREO@email.com");

            Assert.True(result.Success);
        }
        #endregion

        #region Pruebas para GetAllUsuariosAsync
        [Fact]
        public async Task GetUsuariosAsync_DebeRetornarUsuarios()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios { Nombre = "Juan", Email = "juan@email.com", Contraseña = "12345678", Telefono = "8090000001", RolId = 1 });
            context.Usuarios.Add(new Usuarios { Nombre = "Maria", Email = "maria@email.com", Contraseña = "12345678", Telefono = "8090000002", RolId = 1 });

            await context.SaveChangesAsync();

            var result = await service.GetAllUsuariosAsync();

            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetUsuariosAsync_DebeRetornarListaVacia()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var result = await service.GetAllUsuariosAsync();

            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetUsuariosAsync_NoDebeSerNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var result = await service.GetAllUsuariosAsync();

            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetUsuariosAsync_DebeContenerUsuario()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Nombre = "Carlos",
                Email = "carlos@email.com",
                Contraseña = "12345678",
                Telefono = "8090000003",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var result = await service.GetAllUsuariosAsync();

            Assert.Contains(result.Data, u => u.Nombre == "Carlos");
        }

        [Fact]
        public async Task GetUsuariosAsync_DebeRetornarColeccion()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var result = await service.GetAllUsuariosAsync();

            Assert.IsAssignableFrom<IEnumerable<Usuarios>>(result.Data);
        }
        #endregion

        #region Pruebas para UpdateUsuarioAsync
        [Fact]
        public async Task UpdateUsuarioAsync_DebeActualizarNombre()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 1,
                Nombre = "Juan",
                Email = "juan@email.com",
                Contraseña = "12345678",
                Telefono = "8090000001",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var usuario = await context.Usuarios.FindAsync(1);
            usuario.Nombre = "Juan Actualizado";

            await service.UpdateUsuarioAsync(usuario);

            var actualizado = await context.Usuarios.FindAsync(1);

            Assert.Equal("Juan Actualizado", actualizado.Nombre);
        }

        [Fact]
        public async Task UpdateUsuarioAsync_DebeActualizarEmail()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 2,
                Nombre = "Pedro",
                Email = "pedro@email.com",
                Contraseña = "12345678",
                Telefono = "8090000002",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var usuario = await context.Usuarios.FindAsync(2);
            usuario.Email = "nuevo@email.com";

            await service.UpdateUsuarioAsync(usuario);

            var actualizado = await context.Usuarios.FindAsync(2);

            Assert.Equal("nuevo@email.com", actualizado.Email);
        }

        [Fact]
        public async Task UpdateUsuarioAsync_NoDebeCambiarId()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 3,
                Nombre = "Luis",
                Email = "luis@email.com",
                Contraseña = "12345678",
                Telefono = "8090000003",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var usuario = await context.Usuarios.FindAsync(3);
            usuario.Nombre = "Luis Modificado";

            await service.UpdateUsuarioAsync(usuario);

            var actualizado = await context.Usuarios.FindAsync(3);

            Assert.Equal(3, actualizado.Id);
        }

        [Fact]
        public async Task UpdateUsuarioAsync_DebeGuardarCambios()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 4,
                Nombre = "Ana",
                Email = "ana@email.com",
                Contraseña = "12345678",
                Telefono = "8090000004",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var usuario = await context.Usuarios.FindAsync(4);
            usuario.Telefono = "8099999999";

            await service.UpdateUsuarioAsync(usuario);

            var actualizado = await context.Usuarios.FindAsync(4);

            Assert.Equal("8099999999", actualizado.Telefono);
        }

        [Fact]
        public async Task UpdateUsuarioAsync_UsuarioActualizadoNoDebeSerNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 5,
                Nombre = "Mario",
                Email = "mario@email.com",
                Contraseña = "12345678",
                Telefono = "8090000005",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var usuario = await context.Usuarios.FindAsync(5);
            usuario.Nombre = "Mario Updated";

            await service.UpdateUsuarioAsync(usuario);

            var actualizado = await context.Usuarios.FindAsync(5);

            Assert.NotNull(actualizado);
        }
        #endregion

        #region Pruebas para DeleteUsuarioAsync
        [Fact]
        public async Task DeleteUsuarioAsync_DebeEliminarUsuario()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 1,
                Nombre = "Carlos",
                Email = "carlos@email.com",
                Contraseña = "12345678",
                Telefono = "8090000001",
                RolId = 1
            });

            await context.SaveChangesAsync();

            await service.DeleteUsuarioAsync(1);

            var usuario = await context.Usuarios.FindAsync(1);

            Assert.Null(usuario);
        }

        [Fact]
        public async Task DeleteUsuarioAsync_DebeReducirCantidadUsuarios()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios { Nombre = "Juan", Email = "juan@email.com", Contraseña = "12345678", Telefono = "809", RolId = 1 });
            context.Usuarios.Add(new Usuarios { Nombre = "Pedro", Email = "pedro@email.com", Contraseña = "12345678", Telefono = "809", RolId = 1 });

            await context.SaveChangesAsync();

            var usuario = context.Usuarios.First();

            await service.DeleteUsuarioAsync(usuario.Id);

            Assert.Single(context.Usuarios);
        }

        [Fact]
        public async Task DeleteUsuarioAsync_UsuarioNoDebeExistir()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 10,
                Nombre = "Jose",
                Email = "jose@email.com",
                Contraseña = "12345678",
                Telefono = "809",
                RolId = 1
            });

            await context.SaveChangesAsync();

            await service.DeleteUsuarioAsync(10);

            var existe = context.Usuarios.Any(u => u.Id == 10);

            Assert.False(existe);
        }

        [Fact]
        public async Task DeleteUsuarioAsync_NoDebeLanzarExcepcion()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            await service.DeleteUsuarioAsync(100);

            Assert.True(true);
        }

        [Fact]
        public async Task DeleteUsuarioAsync_UsuarioNoDebeEstarEnLista()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Id = 7,
                Nombre = "David",
                Email = "david@email.com",
                Contraseña = "12345678",
                Telefono = "809",
                RolId = 1
            });

            await context.SaveChangesAsync();

            await service.DeleteUsuarioAsync(7);

            Assert.DoesNotContain(context.Usuarios, u => u.Id == 7);
        }
        #endregion

        #region Pruebas para Login
        [Fact]
        public async Task Login_DebeRetornarUsuario()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Email = "login@email.com",
                Contraseña = "12345678",
                Nombre = "Login",
                Telefono = "809",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var usuario = new Usuarios
            {
                Email = "login@email.com",
                Contraseña = "12345678"
            };

            var result = await service.Login(usuario);

            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task Login_ContraseñaIncorrecta()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Email = "login@email.com",
                Contraseña = "12345678",
                Nombre = "Login",
                Telefono = "809",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var usuario = new Usuarios
            {
                Email = "login@email.com",
                Contraseña = "wrong"
            };

            var result = await service.Login(usuario);

            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Login_EmailIncorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var usuario = new Usuarios
            {
                Email = "noexiste@email.com",
                Contraseña = "12345678"
            };

            var result = await service.Login(usuario);

            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Login_UsuarioNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var result = await service.Login(null);

            Assert.Null(result.Data);
        }

        [Fact]
        public async Task Login_NoDebeRetornarUsuarioIncorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            context.Usuarios.Add(new Usuarios
            {
                Email = "correct@email.com",
                Contraseña = "12345678",
                Nombre = "Correct",
                Telefono = "809",
                RolId = 1
            });

            await context.SaveChangesAsync();

            var usuario = new Usuarios
            {
                Email = "correct@email.com",
                Contraseña = "wrong"
            };

            var result = await service.Login(usuario);

            Assert.Null(result.Data);
        }
        #endregion
    }
}
