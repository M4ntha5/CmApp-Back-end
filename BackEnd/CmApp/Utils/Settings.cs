using System;

namespace CmApp.Utils
{
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("9b932d71-5bb4-4579-9d69-c61e6dbca245");
        public static string ApiKey { get; set; } = Environment.GetEnvironmentVariable("ApiKey");
        //"aCM97w8H9Z_rNrsCdemkd9S-HV_Gq5Xe";
        public static string CaptchaApiKey { get; set; } = Environment.GetEnvironmentVariable("CaptchaApiKey");
        //"1d53bbbfbc8e66ffbfef9b172fa5d183";
        public static string DefaultImage { get; set; } = "b9daf2e7-e48f-4d77-89c8-b872013ff9a1";

    }
}
