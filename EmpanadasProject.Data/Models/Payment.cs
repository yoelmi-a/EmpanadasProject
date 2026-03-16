namespace EmpanadasProject.Data.Models
{
    public enum PaymentMethod
    {
        Efectivo = 1,
        Tarjeta = 2,
        Transferencia = 3
    }

    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsConfirmed { get; set; }
    }
}
