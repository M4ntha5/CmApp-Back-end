using CmApp.Contracts;
using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Repositories
{
    public class AggregateRepository : IAggregateRepository
    {
        /* private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

         public async Task<List<CarStats>> GetCarStats(DateTime dateFrom, DateTime dateTo, string userEmail)
         {
             if (dateFrom > dateTo)
                 throw new BusinessException("Date from cannot be less than date to!");
             dateTo = dateTo.AddDays(1).AddSeconds(-1);
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
         }*/

        private readonly DatabaseContext _databaseContext;

        public AggregateRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<CarStats>> GetCarStats(DateTime dateFrom, DateTime dateTo, string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<List<CarDisplay>> GetUserCars(string userEmail)
        {
            throw new NotImplementedException();
        }
    }
}
