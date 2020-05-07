using System;

namespace CmApp.Utils
{
    public static class Settings
    {

        // prod
        public static Guid ProjectId { get; set; }
        public static string ApiKey { get; set; }
        public static string CaptchaApiKey { get; set; }
        public static string DefaultImage { get; set; }
        public static string DefaultImageUrl { get; set; }
        public static string WelcomeEmailTemplateId { get; set; }
        public static string PasswordResetEmailTemplateId { get; set; }
        public static string EmailConfirmationTemplateId { get; set; }
        public static string SendGridApiKey { get; set; }
        public static string SenderEmailAddress { get; set; }
        public static string SenderEmailAddressName { get; set; }

    }
}
