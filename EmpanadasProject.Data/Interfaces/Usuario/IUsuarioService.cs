using EmpanadasProject.Data.Entities;
using EmpanadasProject.Data.Base;
using Microsoft.AspNetCore.Identity;

namespace EmpanadasProject.Data.Interfaces.Usuario
{
    public interface IUsuarioService
    {
        public Task<OperationResult<IdentityUser>> AddUsuarioAsync(IdentityUser usuario, string password);
        public Task<OperationResult<IdentityUser>> GetUsuarioByIdAsync(string id);
        public Task<OperationResult<IdentityUser>> GetUsuariosByEmailAsync(string Email);
        public Task<OperationResult<IEnumerable<IdentityUser>>> GetAllUsuariosAsync();
        public Task<OperationResult<IdentityUser>> UpdateUsuarioAsync(IdentityUser usuario);
        public Task<OperationResult<IdentityUser>> DeleteUsuarioAsync(string id);
    }
}
