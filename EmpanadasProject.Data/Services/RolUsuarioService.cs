

using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Services
{
    public class RolUsuarioService : IRolUsuarioService
    {
        private readonly EmpanadasContext _context;

        public RolUsuarioService(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<RolUsuario>> AddRolUsuarioAsync(RolUsuario rolUsuario)
        {
            OperationResult<RolUsuario> result = new OperationResult<RolUsuario>();

            try
            {
                _context.Add(rolUsuario);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Rol del Usuario agregado exitosamente.";
                result.Data = rolUsuario;
            }
            catch (Exception ex) 
            {
                result.Success = false;
                result.Message = $"Error al agregar el Rol del Usuario: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<RolUsuario>> DeleteRolUsuarioAsync(int id)
        {
            OperationResult<RolUsuario> result = new OperationResult<RolUsuario>();

            try
            {
                var RolUsuario = await _context.RolUsuarios.FindAsync(id);

                if (RolUsuario == null) 
                {
                    result.Success = false;
                    result.Message = "Rol de Usuario no encontrado.";
                    result.Data = null;
                    return result;
                }

                _context.RolUsuarios.Remove(RolUsuario);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Rol de Uusario eliminado exitosamente.";
                result.Data = RolUsuario;
                
            }
            catch (Exception ex) 
            {
                result.Success = false;
                result.Message = $"Error al eliminar el Rol del Usuario: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<IEnumerable<RolUsuario>>> GetAllRolUsuariosAsync()
        {
            OperationResult<IEnumerable<RolUsuario>> result = new OperationResult<IEnumerable<RolUsuario>>();

            try
            {
                var RolUsuario = await _context.RolUsuarios.ToListAsync();

                result.Success = true;
                result.Message = "Roles de los  Usuarios obtenidos exitosamente.";
                result.Data = RolUsuario;
            }
            catch (Exception ex) 
            {
                result.Success = false;
                result.Message = $"Error obteniendo los Roles de los Usuarios: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<RolUsuario>> GetRolUsuarioByIdAsync(int id)
        {
            OperationResult<RolUsuario> result = new OperationResult<RolUsuario>();

            try
            {
                var RolUsuario = await _context.RolUsuarios.FindAsync(id);
                if (RolUsuario == null)
                {
                    result.Success = false;
                    result.Message = "Rolde Usuario no encontrado.";
                    result.Data = null;
                    return result;
                }

                result.Success = true;
                result.Message = "Rol de Usuario obtenido exitosamente.";
                result.Data = RolUsuario;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener el Rol del Usuario: {ex.Message}";
                result.Data = null;
            }
            return result;
        }

        public async Task<OperationResult<RolUsuario>> UpdateRolUsuarioAsync(RolUsuario rolUsuario)
        {
            OperationResult<RolUsuario> result = new OperationResult<RolUsuario>();

            try
            {
                var existingUsuario = await _context.RolUsuarios.FindAsync(rolUsuario.Id);
                if (existingUsuario == null)
                {
                    result.Success = false;
                    result.Message = "Rol del Usuario no encontrado.";
                    result.Data = null;
                    return result;
                }
                existingUsuario.Nombre = rolUsuario.Nombre;

                _context.RolUsuarios.Update(existingUsuario);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Rol del Usuario actualizado exitosamente.";
                result.Data = existingUsuario;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al actualizar el Rol del Usuario: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
    }
}
