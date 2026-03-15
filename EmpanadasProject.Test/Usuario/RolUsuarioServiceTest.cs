

using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Services;
using EmpanadasProject.Test.BaseContext;
using EmpanadasProject.Web.Validations.Usuario;

namespace EmpanadasProject.Test.Usuario
{
    public class RolUsuarioServiceTest
    {
        private RolUsuarioService CrearServicio(EmpanadasContext context)
        {
            return new RolUsuarioService(context);
        }
        private RolUsuarioValidator CrearValidator()
        {
            return new RolUsuarioValidator();
        }

        #region AddRolUsuarioAsync 
        [Fact]
        public async Task AddRolUsuarioAsync_RolValido_DebeAgregar()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);
            var validator = CrearValidator();

            var rol = new RolUsuario
            {
                Id = 1,
                Nombre = "Administrador"
            };

            var validation = validator.Validate(rol);

            Assert.True(validation.IsValid);

            var result = await service.AddRolUsuarioAsync(rol);

            Assert.True(result.Success);
        }

        [Fact]
        public void RolUsuarioValidator_NombreVacio_DebeFallar()
        {
            var validator = CrearValidator();

            var rol = new RolUsuario
            {
                Id = 1,
                Nombre = ""
            };

            var result = validator.Validate(rol);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void RolUsuarioValidator_NombreMuyCorto_DebeFallar()
        {
            var validator = CrearValidator();

            var rol = new RolUsuario
            {
                Id = 1,
                Nombre = "Ad"
            };

            var result = validator.Validate(rol);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void RolUsuarioValidator_NombreMuyLargo_DebeFallar()
        {
            var validator = CrearValidator();

            var rol = new RolUsuario
            {
                Id = 1,
                Nombre = new string('A', 60)
            };

            var result = validator.Validate(rol);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void RolUsuarioValidator_IdInvalido_DebeFallar()
        {
            var validator = CrearValidator();

            var rol = new RolUsuario
            {
                Id = 0,
                Nombre = "Administrador"
            };

            var result = validator.Validate(rol);

            Assert.False(result.IsValid);
        }
        #endregion


        #region DeleteRolUsuarioAsync
        [Fact]
        public async Task DeleteRolUsuarioAsync_RolExiste_DebeEliminar()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 1, Nombre = "Admin" });
            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.DeleteRolUsuarioAsync(1);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteRolUsuarioAsync_DebeRemoverRol()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 2, Nombre = "Empleado" });
            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            await service.DeleteRolUsuarioAsync(2);

            var rol = context.RolUsuarios.FirstOrDefault(r => r.Id == 2);

            Assert.Null(rol);
        }

        [Fact]
        public async Task DeleteRolUsuarioAsync_RolNoExiste()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.DeleteRolUsuarioAsync(10);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeleteRolUsuarioAsync_NoDebeRetornarNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 3, Nombre = "Cliente" });
            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.DeleteRolUsuarioAsync(3);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteRolUsuarioAsync_ListaDebeQuedarVacia()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 5, Nombre = "Administrador" });
            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            await service.DeleteRolUsuarioAsync(5);

            Assert.Empty(context.RolUsuarios);
        }
        #endregion


        #region GetRolUsuarioByIdAsync
        [Fact]
        public async Task GetRolUsuarioByIdAsync_DebeRetornarRol()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 1, Nombre = "Admin" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.GetRolUsuarioByIdAsync(1);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetRolUsuarioByIdAsync_IdCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 3, Nombre = "Empleado" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.GetRolUsuarioByIdAsync(3);

            Assert.Equal(3, result.Data.Id);
        }

        [Fact]
        public async Task GetRolUsuarioByIdAsync_RolNoExiste()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetRolUsuarioByIdAsync(100);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetRolUsuarioByIdAsync_DataDebeSerNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetRolUsuarioByIdAsync(20);

            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetRolUsuarioByIdAsync_DebeRetornarNombre()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 5, Nombre = "Cliente" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.GetRolUsuarioByIdAsync(5);

            Assert.Equal("Cliente", result.Data.Nombre);
        }
        #endregion


        #region GetAllRolUsuariosAsync
        [Fact]
        public async Task GetAllRolUsuariosAsync_DebeRetornarLista()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 1, Nombre = "Admin" });
            context.RolUsuarios.Add(new RolUsuario { Id = 2, Nombre = "Empleado" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.GetAllRolUsuariosAsync();

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetAllRolUsuariosAsync_DebeRetornarTodos()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 1, Nombre = "Admin" });
            context.RolUsuarios.Add(new RolUsuario { Id = 2, Nombre = "Empleado" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.GetAllRolUsuariosAsync();

            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetAllRolUsuariosAsync_SinRegistros()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetAllRolUsuariosAsync();

            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetAllRolUsuariosAsync_DataNoDebeSerNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var result = await service.GetAllRolUsuariosAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllRolUsuariosAsync_DebeContenerNombreCorrecto()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 1, Nombre = "Administrador" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var result = await service.GetAllRolUsuariosAsync();

            Assert.Contains(result.Data, r => r.Nombre == "Administrador");
        }
        #endregion


        #region UpdateRolUsuarioAsync
        [Fact]
        public async Task UpdateRolUsuarioAsync_DebeActualizarNombre()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 1, Nombre = "Admin" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var rol = new RolUsuario { Id = 1, Nombre = "Administrador" };

            var result = await service.UpdateRolUsuarioAsync(rol);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateRolUsuarioAsync_DebeCambiarNombre()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 2, Nombre = "Empleado" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var rol = new RolUsuario { Id = 2, Nombre = "Supervisor" };

            await service.UpdateRolUsuarioAsync(rol);

            var actualizado = context.RolUsuarios.First(r => r.Id == 2);

            Assert.Equal("Supervisor", actualizado.Nombre);
        }

        [Fact]
        public async Task UpdateRolUsuarioAsync_RolNoExiste()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            var service = CrearServicio(context);

            var rol = new RolUsuario { Id = 10, Nombre = "NuevoRol" };

            var result = await service.UpdateRolUsuarioAsync(rol);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateRolUsuarioAsync_NoDebeRetornarNull()
        {
            var factory = new DbEmpanadasContext();
            var context = factory.GetInMemoryContext();

            context.RolUsuarios.Add(new RolUsuario { Id = 3, Nombre = "Cliente" });

            await context.SaveChangesAsync();

            var service = CrearServicio(context);

            var rol = new RolUsuario { Id = 3, Nombre = "ClienteVIP" };

            var result = await service.UpdateRolUsuarioAsync(rol);

            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateRolUsuarioValidator_NombreInvalido()
        {
            var validator = CrearValidator();

            var rol = new RolUsuario
            {
                Id = 1,
                Nombre = "A"
            };

            var result = validator.Validate(rol);

            Assert.False(result.IsValid);
        }
        #endregion


    }
}
