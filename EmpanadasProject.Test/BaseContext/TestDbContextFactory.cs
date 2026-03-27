using EmpanadasProject.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmpanadasProject.Test.BaseContext
{
    public static class TestDbContextFactory
    {
        public static EmpanadasContext Create()
        {
            var options = new DbContextOptionsBuilder<EmpanadasContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new EmpanadasContext(options);
        }
    }
}
