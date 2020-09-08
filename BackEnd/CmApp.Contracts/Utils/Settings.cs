using System;

namespace CmApp.Utils
{
    public static class Settings
    {
        public static string Image4IoApiKey { get; set; } = "VCpoVQjWHblJ0/5/nRaxoQ==";
        public static string Image4IoSecret { get; set; } = "GZ3/NKR+237ZMiY4fmPfcotHPf7DYrizFef2aJ6JwqY=";

        public static Guid ProjectId { get; set; } = Guid.Parse("9b932d71-5bb4-4579-9d69-c61e6dbca245");
        public static string ApiKey { get; set; } = "euHlhRE-TGmz4Y5gtmNeoFm1L5e_Xkun";
        public static string CaptchaApiKey { get; set; } = "1d53bbbfbc8e66ffbfef9b172fa5d183";
        public static string DefaultImageUrl { get; set; } = "https://cdn.image4.io/cmapp//272f3362-1457-4aa9-89c4-491c5994149e.jpg";
        public static string WelcomeEmailTemplateId { get; set; } = "d-ad30b9067e184a9e894774a0d2273fd0";
        public static string PasswordResetEmailTemplateId { get; set; } = "d-b08639c3bb1c4804ba69a2a09d3b5123";
        public static string EmailConfirmationTemplateId { get; set; } = "d-99633e1621ec45a6bc60c31cc0491f39";
        public static string SendGridApiKey { get; set; } = "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";
        public static string SenderEmailAddress { get; set; } = "mantas.daunoravicius@ktu.edu";
        public static string SenderEmailAddressName { get; set; } = "CmApp";
        public static string AdminKey { get; set; } = "this_is_admin_key";
        public static string UserKey { get; set; } = "this_is_user_key";
    }
}
