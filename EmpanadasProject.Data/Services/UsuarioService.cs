using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsuarioService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<OperationResult<IdentityUser>> AddUsuarioAsync(IdentityUser usuario, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(usuario, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return OperationResult<IdentityUser>.Fallido($"Error al agregar el usuario: {errors}");
                }
                return OperationResult<IdentityUser>.Exitoso(usuario);
            }
            catch (Exception ex)
            {
                return OperationResult<IdentityUser>.Fallido($"Error al agregar el usuario: {ex.Message}");
            }
        }

        public async Task<OperationResult<IdentityUser>> GetUsuarioByIdAsync(string id)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(id);
                if (usuario == null)
                {
                    return OperationResult<IdentityUser>.Fallido("Usuario no encontrado.");
                }
                return OperationResult<IdentityUser>.Exitoso(usuario);
            }
            catch (Exception ex)
            {
                return OperationResult<IdentityUser>.Fallido($"Error al obtener el usuario: {ex.Message}");
            }
        }

        public async Task<OperationResult<IdentityUser>> GetUsuariosByEmailAsync(string email)
        {
            try
            {
                var usuario = await _userManager.FindByEmailAsync(email);
                if (usuario == null)
                {
                    return OperationResult<IdentityUser>.Fallido("No se encontró el usuario con ese email.");
                }
                return OperationResult<IdentityUser>.Exitoso(usuario);
            }
            catch (Exception ex)
            {
                return OperationResult<IdentityUser>.Fallido($"Error al obtener el usuario: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<IdentityUser>>> GetAllUsuariosAsync()
        {
            try
            {
                var usuarios = await _userManager.Users.ToListAsync();
                return OperationResult<IEnumerable<IdentityUser>>.Exitoso(usuarios);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<IdentityUser>>.Fallido($"Error al obtener los usuarios: {ex.Message}");
            }
        }

        public async Task<OperationResult<IdentityUser>> UpdateUsuarioAsync(IdentityUser usuario)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(usuario.Id);
                if (existingUser == null)
                {
                    return OperationResult<IdentityUser>.Fallido("Usuario no encontrado.");
                }

                existingUser.UserName = usuario.UserName;
                existingUser.Email = usuario.Email;
                existingUser.PhoneNumber = usuario.PhoneNumber;

                var result = await _userManager.UpdateAsync(existingUser);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return OperationResult<IdentityUser>.Fallido($"Error al actualizar el usuario: {errors}");
                }
                return OperationResult<IdentityUser>.Exitoso(existingUser);
            }
            catch (Exception ex)
            {
                return OperationResult<IdentityUser>.Fallido($"Error al actualizar el usuario: {ex.Message}");
            }
        }

        public async Task<OperationResult<IdentityUser>> DeleteUsuarioAsync(string id)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(id);
                if (usuario == null)
                {
                    return OperationResult<IdentityUser>.Fallido("Usuario no encontrado.");
                }
                var result = await _userManager.DeleteAsync(usuario);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return OperationResult<IdentityUser>.Fallido($"Error al eliminar el usuario: {errors}");
                }
                return OperationResult<IdentityUser>.Exitoso(usuario);
            }
            catch (Exception ex)
            {
                return OperationResult<IdentityUser>.Fallido($"Error al eliminar el usuario: {ex.Message}");
            }
        }
    }
}
