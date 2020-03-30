using System;

namespace CmApp.Utils
{
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("9b932d71-5bb4-4579-9d69-c61e6dbca245");
        public static string ApiKey { get; set; } = "euHlhRE-TGmz4Y5gtmNeoFm1L5e_Xkun"; //Environment.GetEnvironmentVariable("ApiKey");

        public static string CaptchaApiKey { get; set; } = "1d53bbbfbc8e66ffbfef9b172fa5d183"; //Environment.GetEnvironmentVariable("CaptchaApiKey");

        public static string DefaultImage { get; set; } = "b9daf2e7-e48f-4d77-89c8-b872013ff9a1";

    }
}
