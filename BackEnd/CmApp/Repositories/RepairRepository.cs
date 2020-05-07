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
    public class RepairRepository : IRepairRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<RepairEntity> InsertRepair(RepairEntity repair)
        {
            if (repair == null)
            {
                throw new ArgumentNullException(nameof(repair), "Cannot insert repair in db, because repair is empty");
            }

            var repo = new CodeMashRepository<RepairEntity>(Client);

            var response = await repo.InsertOneAsync(repair, new DatabaseInsertOneOptions());
            return response;
        }
        public async Task InsertMultipleRepairs(List<RepairEntity> repairs)
        {
            if (repairs == null)
            {
                throw new ArgumentNullException(nameof(repairs), "Cannot insert repairs in db, because repairs is empty");
            }

            var repo = new CodeMashRepository<RepairEntity>(Client);

            var entities = repairs
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 100)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            foreach (var ent in entities)
                await repo.InsertManyAsync(ent, new DatabaseInsertManyOptions());

        }

        public async Task<List<RepairEntity>> GetAllRepairsByCarId(string carId)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);
            var filter = Builders<RepairEntity>.Filter.Eq("car", BsonObjectId.Create(carId));

            var repairs = await repo.FindAsync(filter, new DatabaseFindOptions());
            return repairs.Items;
        }


        public async Task<RepairEntity> GetCarRepairById(string carId, string repairId)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);

            FilterDefinition<RepairEntity>[] filters =
            {
                Builders<RepairEntity>.Filter.Eq("car",  BsonObjectId.Create(carId)),
                Builders<RepairEntity>.Filter.Eq("_id",  BsonObjectId.Create(repairId))
            };

            var filter = Builders<RepairEntity>.Filter.And(filters);

            var repair = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());
            return repair;
        }

        public async Task DeleteMultipleRepairs(string carId)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);

            var filter = Builders<RepairEntity>.Filter.Eq("car", ObjectId.Parse(carId));

            await repo.DeleteManyAsync(filter);
        }

        public async Task UpdateRepair(string repairId, RepairEntity repair)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);
            repair.Id = repairId;

            await repo.ReplaceOneAsync(
                x => x.Id == repairId,
                repair,
                new DatabaseReplaceOneOptions()
            );
        }
    }
}
