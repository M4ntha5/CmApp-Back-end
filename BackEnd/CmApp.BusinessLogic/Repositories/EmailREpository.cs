using CmApp.Contracts;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Utils;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly SendGridClient Client = new SendGridClient(Settings.SendGridApiKey);

        public async Task SendWelcomeEmail(string email)
        {
            var message = new SendGridMessage();
            message.SetFrom(Settings.SenderEmailAddress, Settings.SenderEmailAddressName);
            message.AddTo(email);
            message.SetTemplateId(Settings.WelcomeEmailTemplateId);

            await Client.SendEmailAsync(message);
        }
        public async Task SendEmailConfirmationEmail(string email, string token)
        {
            var message = new SendGridMessage();
            message.SetFrom(Settings.SenderEmailAddress, Settings.SenderEmailAddressName);
            message.AddTo(email);
            message.SetTemplateId(Settings.EmailConfirmationTemplateId);
            message.SetTemplateData(new { token });

            await Client.SendEmailAsync(message);
        }

        public async Task SendPasswordResetEmail(string email, string token)
        {
            var message = new SendGridMessage();
            message.SetFrom(Settings.SenderEmailAddress, Settings.SenderEmailAddressName);
            message.AddTo(email);
            message.SetTemplateId(Settings.PasswordResetEmailTemplateId);
            message.SetTemplateData(new { token });

            await Client.SendEmailAsync(message);
        }
    }
}
