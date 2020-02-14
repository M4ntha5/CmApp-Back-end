using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Utils
{
    public static class Settings
    {
        public static Guid ProjectId { get; set; } = Guid.Parse("9b932d71-5bb4-4579-9d69-c61e6dbca245");
        public static string ApiKey { get; set; } = "aCM97w8H9Z_rNrsCdemkd9S-HV_Gq5Xe";//Environment.GetEnvironmentVariable("ApiKey");

        public static string CarsCollectionName { get; set; } = "Cars";
        public static string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public static string DatabaseName { get; set; } = "CarsManagment";
    }
}
