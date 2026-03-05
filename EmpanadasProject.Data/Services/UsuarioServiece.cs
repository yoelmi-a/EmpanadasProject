using EmpanadasProject.Data.Contexts;
using EmpanadasProject.Data.Entities.Usuario;
using EmpanadasProject.Data.Interfaces;
using EmpanadasProject.Data.OperationResult;

namespace EmpanadasProject.Data.Repositories
{
    public class UsuarioServiece : IUsuarioService
    {
        private readonly EmpanadasContext _context;

        public UsuarioServiece(EmpanadasContext context)
        {
            _context = context;
        }

        public Task<OperationResult<Usuarios>> AddUsuarioAsync(Usuarios usuario)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Usuarios>> DeleteUsuarioAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IEnumerable<Usuarios>>> GetAllUsuariosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Usuarios>> GetUsuarioByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Usuarios>> Login(Usuarios usuario)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Usuarios>> UpdateUsuarioAsync(Usuarios usuario)
        {
            throw new NotImplementedException();
        }
    }
}
