using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Base;
using Microsoft.AspNetCore.Identity;

namespace EmpanadasProject.Data.Interfaces.Usuario
{
    public interface IRolUsuarioService
    {
        public Task<OperationResult<IdentityRole>> AddRolUsuarioAsync(IdentityRole rolUsuario);
        public Task<OperationResult<IdentityRole>> GetRolUsuarioByIdAsync(string id);
        public Task<OperationResult<IEnumerable<IdentityRole>>> GetAllRolUsuariosAsync();
        public Task<OperationResult<IdentityRole>> UpdateRolUsuarioAsync(IdentityRole rolUsuario);
        public Task<OperationResult<IdentityRole>> DeleteRolUsuarioAsync(string id);
    }
}
