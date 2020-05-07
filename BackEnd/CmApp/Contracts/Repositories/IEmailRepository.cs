using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IEmailRepository
    {
        Task SendWelcomeEmail(string email);
        Task SendEmailConfirmationEmail(string email, string token);
        Task SendPasswordResetEmail(string email, string token);
    }
}
