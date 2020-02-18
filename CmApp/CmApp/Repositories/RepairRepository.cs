using CmApp.Contracts;
using CmApp.Domains;
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

        public async Task<RepairEntity> InsertRepair(Repair repair)
        {
            if (repair == null)
            {
                throw new ArgumentNullException(nameof(repair), "Cannot insert repair in db, because repair is empty");
            }

            var repo = new CodeMashRepository<RepairEntity>(Client);

            var entity = new RepairEntity
            {
                Name = repair.Name,
                Price = repair.Price,
                Car = repair.Car.Id
            };
            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response;
        }

        public async Task<List<RepairEntity>> GetAllRepairs()
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);

            var repairs = await repo.FindAsync(x => true, new DatabaseFindOptions());
            return repairs.Items;
        }
        public async Task<List<RepairEntity>> GetAllRepairsByCarId(string carId)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);
            var filter = Builders<RepairEntity>.Filter.Eq("car", BsonObjectId.Create(carId));

            var repairs = await repo.FindAsync(filter, new DatabaseFindOptions());
            return repairs.Items;
        }

        public async Task<RepairEntity> GetRepairById(string repairId)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);

            var repair = await repo.FindOneAsync(x => x.Id == repairId, new DatabaseFindOneOptions());
            return repair;
        }
        public async Task UpdateRepair(string repairId, Repair repair)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);

            var entity = new RepairEntity
            {
                Name = repair.Name,
                Price = repair.Price,
                Car = repair.Car.Id
            };

            await repo.ReplaceOneAsync(
                x => x.Id == repairId,
                entity,
                new DatabaseReplaceOneOptions()
            );

        }

        public async Task DeleteRepair(string id)
        {
            var repo = new CodeMashRepository<RepairEntity>(Client);

            await repo.DeleteOneAsync(x => x.Id == id);

        }

    }
}
