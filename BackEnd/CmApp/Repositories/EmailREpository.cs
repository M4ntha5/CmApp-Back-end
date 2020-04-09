using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Notifications.Email.Services;
using Isidos.CodeMash.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class EmailRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task SendUserConfirmationEmail(List<string> emails)
        {
            var emailService = new CodeMashEmailService(Client);

            var response = await emailService.SendEmailAsync(new SendEmailRequest
            {
                TemplateId = Guid.Parse("1fb809cc-ea7f-47a3-a7ce-9b2b2c1168a8"),
                Emails = emails
            });
        }
    }
}
