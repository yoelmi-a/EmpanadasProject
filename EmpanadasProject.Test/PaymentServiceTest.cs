using EmpanadasProject.Data.Models;
using EmpanadasProject.Data.Services;
using Xunit;

namespace EmpanadasProject.Test
{
    public class PaymentServiceTest
    {
        [Fact]
        public void Create_ShouldReturnPayment()
        {
            var service = new PaymentService();

            var result = service.Create(300, PaymentMethod.Efectivo);

            Assert.NotNull(result);
            Assert.Equal(300, result.Amount);
            Assert.True(result.IsConfirmed);
        }
    }
}