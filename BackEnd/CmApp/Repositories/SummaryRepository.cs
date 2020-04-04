using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class SummaryRepository : ISummaryRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<SummaryEntity> InsertSummary(SummaryEntity summary)
        {
            if (summary == null)
            {
                throw new ArgumentNullException(nameof(summary), "Cannot insert summary in db, because summary is empty");
            }

            var repo = new CodeMashRepository<SummaryEntity>(Client);

            var response = await repo.InsertOneAsync(summary, new DatabaseInsertOneOptions());
            return response;
        }

        public async Task<SummaryEntity> GetSummaryByCarId(string carId)
        {
            var repo = new CodeMashRepository<SummaryEntity>(Client);

            var filter = Builders<SummaryEntity>.Filter.Eq("car", BsonObjectId.Create(carId));

            var summary = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());

            return summary;
        }

        public async Task DeleteCarSummary(string carId)
        {
            var repo = new CodeMashRepository<SummaryEntity>(Client);

            FilterDefinition<SummaryEntity>[] filters =
            {
                Builders<SummaryEntity>.Filter.Eq("car",  BsonObjectId.Create(carId)),
            };

            var filter = Builders<SummaryEntity>.Filter.And(filters);

            await repo.DeleteOneAsync(filter);
        }

        public async Task UpdateCarSoldSummary(SummaryEntity summary)
        {
            var repo = new CodeMashRepository<SummaryEntity>(Client);

            var update = Builders<SummaryEntity>.Update
                .Set("sold", summary.Sold)
                .Set("sold_price", summary.SoldPrice)
                .Set("sold_date", summary.SoldDate)
                .Set("sold_within", summary.SoldWithin);

            await repo.UpdateOneAsync(
                summary.Id,
                update,
                new DatabaseUpdateOneOptions()
            );
        }
        public async Task InsertTotalByCar(string summaryId, double total)
        {
            var repo = new CodeMashRepository<SummaryEntity>(Client);

            var update = Builders<SummaryEntity>.Update.Set("total", Math.Round(total,2));

            await repo.UpdateOneAsync(
                x => x.Id == summaryId,
                update,
                new DatabaseUpdateOneOptions()
            );
        }
    }
}
