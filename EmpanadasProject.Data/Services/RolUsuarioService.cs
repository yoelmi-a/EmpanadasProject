using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class RolUsuarioService : IRolUsuarioService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolUsuarioService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<OperationResult<IdentityRole>> AddRolUsuarioAsync(IdentityRole rolUsuario)
        {
            try
            {
                var result = await _roleManager.CreateAsync(rolUsuario);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return OperationResult<IdentityRole>.Fallido($"Error al agregar el rol: {errors}");
                }
                return OperationResult<IdentityRole>.Exitoso(rolUsuario);
            }
            catch (Exception ex) 
            {
                return OperationResult<IdentityRole>.Fallido($"Error al agregar el rol: {ex.Message}");
            }
        }

        public async Task<OperationResult<IdentityRole>> DeleteRolUsuarioAsync(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) 
                {
                    return OperationResult<IdentityRole>.Fallido("Rol no encontrado.");
                }

                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return OperationResult<IdentityRole>.Fallido($"Error al eliminar el rol: {errors}");
                }
                return OperationResult<IdentityRole>.Exitoso(role);
            }
            catch (Exception ex) 
            {
                return OperationResult<IdentityRole>.Fallido($"Error al eliminar el rol: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<IdentityRole>>> GetAllRolUsuariosAsync()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                return OperationResult<IEnumerable<IdentityRole>>.Exitoso(roles);
            }
            catch (Exception ex) 
            {
                return OperationResult<IEnumerable<IdentityRole>>.Fallido($"Error obteniendo los roles: {ex.Message}");
            }
        }

        public async Task<OperationResult<IdentityRole>> GetRolUsuarioByIdAsync(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return OperationResult<IdentityRole>.Fallido("Rol no encontrado.");
                }
                return OperationResult<IdentityRole>.Exitoso(role);
            }
            catch (Exception ex)
            {
                return OperationResult<IdentityRole>.Fallido($"Error al obtener el rol: {ex.Message}");
            }
        }

        public async Task<OperationResult<IdentityRole>> UpdateRolUsuarioAsync(IdentityRole rolUsuario)
        {
            try
            {
                var existingRole = await _roleManager.FindByIdAsync(rolUsuario.Id);
                if (existingRole == null)
                {
                    return OperationResult<IdentityRole>.Fallido("Rol no encontrado.");
                }
                
                existingRole.Name = rolUsuario.Name;
                var result = await _roleManager.UpdateAsync(existingRole);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return OperationResult<IdentityRole>.Fallido($"Error al actualizar el rol: {errors}");
                }
                return OperationResult<IdentityRole>.Exitoso(existingRole);
            }
            catch (Exception ex)
            {
                return OperationResult<IdentityRole>.Fallido($"Error al actualizar el rol: {ex.Message}");
            }
        }
    }
}
