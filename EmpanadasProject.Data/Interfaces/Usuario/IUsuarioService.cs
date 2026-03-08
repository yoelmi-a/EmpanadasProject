using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.OperationResult;

namespace EmpanadasProject.Data.Interfaces.Usuario
{
    public interface IUsuarioService
    {
        public Task<OperationResult<Usuarios>> AddUsuarioAsync(Usuarios usuario);
        public Task<OperationResult<Usuarios>> GetUsuarioByIdAsync(int id);
        public Task<OperationResult<Usuarios>> GetUsuariosByEmailAsync(string Email);
        public Task<OperationResult<IEnumerable<Usuarios>>> GetAllUsuariosAsync();
        public Task<OperationResult<Usuarios>> UpdateUsuarioAsync(Usuarios usuario);
        public Task<OperationResult<Usuarios>> DeleteUsuarioAsync(int id);
        public Task<OperationResult<Usuarios>> Login(Usuarios usuario);
    }
}
