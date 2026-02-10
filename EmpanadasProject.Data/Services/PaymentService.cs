using EmpanadasProject.Data.Models;

namespace EmpanadasProject.Data.Services
{
    public class PaymentService
    {
        private static readonly List<Payment> payments = new();

        public Payment Create(decimal amount, PaymentMethod method)
        {
            var payment = new Payment
            {
                Id = payments.Count + 1,
                Amount = amount,
                Method = method,
                IsConfirmed = true
            };

            payments.Add(payment);
            return payment;
        }

        public List<Payment> GetAll()
        {
            return payments;
        }
    }
}
