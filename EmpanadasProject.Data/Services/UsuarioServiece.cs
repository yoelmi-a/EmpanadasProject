using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Interfaces.Usuario;
using EmpanadasProject.Data.OperationResult;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Repositories
{
    public class UsuarioServiece : IUsuarioService
    {
        private readonly EmpanadasContext _context;

        public UsuarioServiece(EmpanadasContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Usuarios>> AddUsuarioAsync(Usuarios usuario)
        {
            OperationResult<Usuarios> result = new OperationResult<Usuarios>();
            
            try
            {
               usuario.FechaDeRegistro = DateTime.Now;
               _context.Usuarios.Add(usuario);
               await _context.SaveChangesAsync();


                result.Success = true;
                result.Message = "Usuario agregado exitosamente.";
                result.Data = usuario;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al agregar el usuario: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
        public async Task<OperationResult<Usuarios>> GetUsuarioByIdAsync(int id)
        {
            OperationResult<Usuarios> result = new OperationResult<Usuarios>(); 
            try
            {
                var usuario =  await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    result.Data = null;
                    return result;
                }
                result.Success = true;
                result.Message = "Usuario obtenido exitosamente.";
                result.Data = usuario;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener el usuario: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
        public async Task<OperationResult<Usuarios>> DeleteUsuarioAsync(int id)
        {
            OperationResult<Usuarios> result = new OperationResult<Usuarios>();

            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    result.Data = null;
                    return result;
                }
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Usuario eliminado exitosamente.";
                result.Data = usuario;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al eliminar el usuario: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
        public async Task<OperationResult<IEnumerable<Usuarios>>> GetAllUsuariosAsync()
        {
            OperationResult<IEnumerable<Usuarios>> result = new OperationResult<IEnumerable<Usuarios>>();
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                result.Success = true;
                result.Message = "Usuarios obtenidos exitosamente.";
                result.Data = usuarios;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener los usuarios: {ex.Message}";
                result.Data = null;
            }
            return  result;
        }
        public async Task<OperationResult<Usuarios>> GetUsuariosByEmailAsync(string Email)
        {
            OperationResult<Usuarios> result = new OperationResult<Usuarios>();

            try
            {
                var usuarios = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == Email);
                if (usuarios == null)
                {
                    result.Success = false;
                    result.Message = "No se encontraron usuarios con ese email.";
                    result.Data = null;

                    return result;
                }
                result.Success = true;
                result.Message = "Usuarios obtenidos exitosamente.";
                result.Data = usuarios;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al obtener los usuarios: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
        public async Task<OperationResult<Usuarios>> Login(Usuarios usuario)
        {
            OperationResult<Usuarios> result = new OperationResult<Usuarios>();
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Contraseña == usuario.Contraseña);
                if (user == null)
                {
                    result.Success = false;
                    result.Message = "Credenciales incorrectas.";
                    result.Data = null;
                }
                result.Success = true;
                result.Message = "Login exitoso.";
                result.Data = user;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al realizar el login: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
        public async Task<OperationResult<Usuarios>> UpdateUsuarioAsync(Usuarios usuario)
        {
            OperationResult<Usuarios> result = new OperationResult<Usuarios>();
            try
            {
                var existingUsuario = await _context.Usuarios.FindAsync(usuario.Id);
                if (existingUsuario == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    result.Data = null;
                    return result;
                }
                existingUsuario.Nombre = usuario.Nombre;
                existingUsuario.Email = usuario.Email;
                existingUsuario.Contraseña = usuario.Contraseña;
                existingUsuario.Telefono = usuario.Telefono;
                existingUsuario.RolId = usuario.RolId;
                _context.Usuarios.Update(existingUsuario);
                await _context.SaveChangesAsync();
                result.Success = true;
                result.Message = "Usuario actualizado exitosamente.";
                result.Data = existingUsuario;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error al actualizar el usuario: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
    }
}
