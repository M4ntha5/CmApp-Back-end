using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class AggregateRepository : IAggregateRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<List<CarStats>> GetCarStats(DateTime dateFrom, DateTime dateTo, string userEmail)
        {
            var from = ((DateTimeOffset)dateFrom).ToUnixTimeMilliseconds();
            var to = ((DateTimeOffset)dateTo).ToUnixTimeMilliseconds();

            var service = new CodeMashRepository<SummaryEntity>(Client);
            var stats = await service.AggregateAsync<CarStats>(
                Guid.Parse("d40009f0-3d57-4ecc-bad0-8151faa24041"),
                new AggregateOptions
                {
                    Tokens = new Dictionary<string, string>()
                    {
                           { "dateFrom", from.ToString() },
                           { "dateTo",  to.ToString() },
                           { "userEmail", userEmail },
                    }
                });

            return stats;
        }

        public async Task<List<CarDisplay>> GetUserCars(string userEmail)
        {
            var service = new CodeMashRepository<CarEntity>(Client);

            var cars = await service.AggregateAsync<CarDisplay>(
                Guid.Parse("3c84eeb5-8987-42f9-8033-d597ca76880a"),
                new AggregateOptions
                {
                    Tokens = new Dictionary<string, string>()
                    {
                           { "userEmail", userEmail },
                    }
                });

            return cars;
        }
    }
}
