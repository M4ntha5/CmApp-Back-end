using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IEmailRepository
    {
        Task SendWelcomeEmail(string email);
        Task SendEmailConfirmationEmail(string email, int token);
        Task SendPasswordResetEmail(string email, int token);
    }
}
