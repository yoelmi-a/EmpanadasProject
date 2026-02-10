using EmpanadasProject.Data.Settings;

namespace EmpanadasProject.Data.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest emailRequest);
    }
}
