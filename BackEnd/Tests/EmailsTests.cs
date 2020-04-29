using CmApp.Repositories;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EmailsTests
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

            var email = "mantozerix@gmail.com";

            await repo.SendEmailConfirmationEmail(email, "my-token");
        }
        [Test]
        public async Task SendPasswordResetEmail()
        {
            var repo = new EmailRepository();

            var email = "mantozerix@gmail.com";

            await repo.SendPasswordResetEmail(email, "my-token");
        }
        [Test]
        public async Task SendWelcomeEmail()
        {
            var repo = new EmailRepository();

            var email = "mantozerix@gmail.com";

            await repo.SendWelcomeEmail(email);
        }
    }
}
