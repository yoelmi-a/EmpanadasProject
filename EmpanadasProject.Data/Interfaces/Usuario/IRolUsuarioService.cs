using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.OperationResult;

namespace EmpanadasProject.Data.Interfaces.Usuario
{
    public interface IRolUsuarioService
    {
        public Task<OperationResult<RolUsuario>> AddRolUsuarioAsync(RolUsuario rolUsuario);
        public Task<OperationResult<RolUsuario>> GetRolUsuarioByIdAsync(int id);
        public Task<OperationResult<IEnumerable<RolUsuario>>> GetAllRolUsuariosAsync();
        public Task<OperationResult<RolUsuario>> UpdateRolUsuarioAsync(RolUsuario rolUsuario);
        public Task<OperationResult<RolUsuario>> DeleteRolUsuarioAsync(int id);
    }
}
