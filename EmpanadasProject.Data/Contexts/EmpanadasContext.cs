using EmpanadasProject.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Data.Contexts
{
    public class EmpanadasContext : DbContext
    {
        public EmpanadasContext(DbContextOptions<EmpanadasContext> options) : base(options)
        {            
        }

        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
