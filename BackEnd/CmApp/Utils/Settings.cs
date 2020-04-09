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
