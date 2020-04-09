using CmApp.Repositories;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emails.Tests
{
    class EmailsTests
    {
        [SetUp]
        public void SetUp ()
        {

        }

        [Test]
        public async Task SendConfirmationEmail()
        {
            var repo = new EmailRepository();

            var emails = new List<string> { "mantozerix@gmail.com" };

            await repo.SendUserConfirmationEmail(emails);

        }
    }
}
