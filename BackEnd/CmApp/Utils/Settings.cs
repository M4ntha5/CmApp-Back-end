using System;

namespace CmApp.Utils
{
    public static class Settings
    {

        // old cmapp

        public static Guid ProjectId { get; set; } = Guid.Parse("9b932d71-5bb4-4579-9d69-c61e6dbca245");
        public static string ApiKey { get; set; } = "euHlhRE-TGmz4Y5gtmNeoFm1L5e_Xkun"; //Environment.GetEnvironmentVariable("ApiKey");

        public static string CaptchaApiKey { get; set; } = "1d53bbbfbc8e66ffbfef9b172fa5d183"; //Environment.GetEnvironmentVariable("CaptchaApiKey");

        public static string DefaultImage { get; set; } = "b9daf2e7-e48f-4d77-89c8-b872013ff9a1";
        public static string DefaultImageUrl { get; set; } =
                                                            "https://cm-9b932d71-5bb4-4579-9d69-c61e6dbca245.s3.eu-central-1.amazonaws.com/b9daf2e7-e48f-4d77-89c8-b872013ff9a1.jpg";

        public static string WelcomeEmailTemplateId { get; set; } = "d-ad30b9067e184a9e894774a0d2273fd0";
        public static string PasswordResetEmailTemplateId { get; set; } = "d-b08639c3bb1c4804ba69a2a09d3b5123";
        public static string EmailConfirmationTemplateId { get; set; } = "d-99633e1621ec45a6bc60c31cc0491f39";
        public static string SendGridApiKey { get; set; } = "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";
        public static string SenderEmailAddress { get; set; } = "mantas.daunoravicius@ktu.edu";
        public static string SenderEmailAddressName { get; set; } = "CmApp";

        /*//new 
        public static Guid ProjectId { get; set; } = Guid.Parse("db915e93-726d-4dd0-b367-e182ff7d24ec");
        public static string ApiKey { get; set; } = "YIFOQ-R7CW9zks1nU8RBinvZBTv7fuu4";
        public static string CaptchaApiKey { get; set; } = "1d53bbbfbc8e66ffbfef9b172fa5d183";
        //Environment.GetEnvironmentVariable("CaptchaApiKey");

            //xujovas sitas
        public static string DefaultImage { get; set; } = "b9daf2e7-e48f-4d77-89c8-b872013ff9a1";
        public static string DefaultImageUrl { get; set; } =
            "https://cm-9b932d71-5bb4-4579-9d69-c61e6dbca245.s3.eu-central-1.amazonaws.com/b9daf2e7-e48f-4d77-89c8-b872013ff9a1.jpg";


        public static string EmailConfirmationTemplateId = "a4a00e79-3fd4-480d-944e-f9e5c1ce8af9";
        */
    }
}
