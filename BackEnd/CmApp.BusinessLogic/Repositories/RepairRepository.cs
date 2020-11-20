using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Repositories
{
    public class RepairRepository : IRepairRepository
    {
        /*  private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

          public async Task<RepairEntity> InsertRepair(RepairEntity repair)
          {
              if (repair == null)
                  throw new ArgumentNullException(nameof(repair), "Cannot insert repair in db, because repair is empty");

              var repo = new CodeMashRepository<RepairEntity>(Client);

              var response = await repo.InsertOneAsync(repair, new DatabaseInsertOneOptions());
              return response;
          }
          public async Task InsertMultipleRepairs(List<RepairEntity> repairs)
          {
              if (repairs == null)
                  throw new ArgumentNullException(nameof(repairs), "Cannot insert repairs in db, because repairs is empty");

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
              repair.ID = repairId;
              repair.Total = Math.Round(repair.Total, 2);
              repair.Price = Math.Round(repair.Price, 2);
              await repo.ReplaceOneAsync(
                  x => x.ID == repairId,
                  repair,
                  new DatabaseReplaceOneOptions()
              );
          }*/

        private readonly Context _context;

        public RepairRepository(Context context)
        {
            _context = context;
        }

        public Task DeleteMultipleRepairs(int carId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Repair>> GetAllRepairsByCarId(int carId)
        {
            throw new NotImplementedException();
        }

        public Task<Repair> GetCarRepairById(int carId, int repairId)
        {
            throw new NotImplementedException();
        }

        public Task InsertMultipleRepairs(List<Repair> repairs)
        {
            throw new NotImplementedException();
        }

        public Task<Repair> InsertRepair(Repair repair)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRepair(int repairId, Repair repair)
        {
            throw new NotImplementedException();
        }
    }
}
