

using EmpanadasProject.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EmpanadasProject.Test.BaseContext
{
    public class DbEmpanadasContext
    {
        public  EmpanadasContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<EmpanadasContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new EmpanadasContext(options);
        }
    }
}
