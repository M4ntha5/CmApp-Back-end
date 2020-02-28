using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<int> DeleteCarSummary(string carId, string summaryId)
        {
            var repo = new CodeMashRepository<SummaryEntity>(Client);

            FilterDefinition<SummaryEntity>[] filters =
            {
                Builders<SummaryEntity>.Filter.Eq("car",  BsonObjectId.Create(carId)),
                Builders<SummaryEntity>.Filter.Eq("_id",  BsonObjectId.Create(summaryId))
            };

            var filter = Builders<SummaryEntity>.Filter.And(filters);

            var response = await repo.DeleteOneAsync(filter);

            return (int)response.DeletedCount;
        }

        public async Task UpdateCarSummary(SummaryEntity summary)
        {
            var repo = new CodeMashRepository<SummaryEntity>(Client);

            var response = await repo.ReplaceOneAsync(
                x => x.Id == summary.Id,
                summary,
                new DatabaseReplaceOneOptions()
            );
        }
    }
}
