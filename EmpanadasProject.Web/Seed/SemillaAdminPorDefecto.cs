using EmpanadasProject.Data.Base;
using EmpanadasProject.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EmpanadasProject.Web.Seed
{
    /// <summary>
    /// Inicializa los roles por defecto y el usuario administrador inicial.
    /// </summary>
    public class SemillaAdminPorDefecto
    {
        private readonly UserManager<IdentityUser> _gestorUsuarios;
        private readonly RoleManager<IdentityRole> _gestorRoles;
        private readonly OpcionesAdminPorDefecto _opciones;
        private readonly ILogger<SemillaAdminPorDefecto> _logger;

        public SemillaAdminPorDefecto(
            UserManager<IdentityUser> gestorUsuarios,
            RoleManager<IdentityRole> gestorRoles,
            IOptions<OpcionesAdminPorDefecto> opciones,
            ILogger<SemillaAdminPorDefecto> logger)
        {
            _gestorUsuarios = gestorUsuarios;
            _gestorRoles = gestorRoles;
            _opciones = opciones.Value;
            _logger = logger;
        }

        /// <summary>
        /// Ejecuta el proceso de semilla de manera idempotente.
        /// </summary>
        /// <returns>Resultado de la operación.</returns>
        public async Task<OperationResult> EjecutarAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando proceso de semilla de roles y administrador.");

                // AC-01: Crear roles Administrador y Cliente
                var roles = new[] { "Administrador", "Cliente" };
                foreach (var nombreRol in roles)
                {
                    if (!await _gestorRoles.RoleExistsAsync(nombreRol))
                    {
                        _logger.LogInformation("Creando rol {Rol}.", nombreRol);
                        await _gestorRoles.CreateAsync(new IdentityRole(nombreRol));
                    }
                }

                // AC-02: Crear Administrador por defecto si no existe ninguno
                var admins = await _gestorUsuarios.GetUsersInRoleAsync("Administrador");
                if (admins.Count == 0)
                {
                    _logger.LogInformation("No se encontró ningún administrador. Creando administrador por defecto: {Email}.", _opciones.Email);

                    var admin = new IdentityUser
                    {
                        UserName = _opciones.Email,
                        Email = _opciones.Email,
                        EmailConfirmed = true
                    };

                    var resultadoCreacion = await _gestorUsuarios.CreateAsync(admin, _opciones.Password);
                    if (!resultadoCreacion.Succeeded)
                    {
                        var errores = string.Join(", ", resultadoCreacion.Errors.Select(e => e.Description));
                        _logger.LogError("Error al crear el administrador por defecto: {Errores}", errores);
                        return OperationResult.Fallido($"No se pudo crear el administrador: {errores}");
                    }

                    await _gestorUsuarios.AddToRoleAsync(admin, "Administrador");
                    _logger.LogInformation("Administrador por defecto creado exitosamente.");
                }
                else
                {
                    _logger.LogInformation("El administrador ya existe. Omitiendo creación.");
                }

                return OperationResult.Exitoso();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante el proceso de semilla.");
                throw; // Dejar que el middleware de excepciones lo maneje o que falle el arranque si es crítico
            }
        }
    }
}
