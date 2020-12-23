using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteMultipleRepairs(int carId)
        {
            var repairs = await _context.Repairs.Where(x => x.CarId == carId).ToListAsync();
            if (repairs != null && repairs.Count > 0)
            {
                _context.Repairs.RemoveRange(repairs);
                await _context.SaveChangesAsync();
            }
        }

        public Task<List<Repair>> GetAllRepairsByCarId(int carId)
        {
            return _context.Repairs.Where(x => x.CarId == carId).ToListAsync();
        }

        public Task<Repair> GetCarRepairById(int carId, int repairId)
        {
            return _context.Repairs.FirstOrDefaultAsync(x => x.Id == repairId && x.CarId == carId);
        }

        public async Task InsertMultipleRepairs(List<Repair> repairs)
        {
            if (repairs == null || repairs.Count < 1)
                throw new ArgumentNullException(nameof(repairs), "Cannot insert repairs in db, because repairs is empty");

            await _context.Repairs.AddRangeAsync(repairs);
            await _context.SaveChangesAsync();
        }

        public async Task<Repair> InsertRepair(Repair repair)
        {
            if (repair == null)
                throw new ArgumentNullException(nameof(repair), "Cannot insert repair in db, because repair is empty");

            await _context.Repairs.AddAsync(repair);
            await _context.SaveChangesAsync();
            return repair;
        }

        public async Task UpdateRepair(int repairId, Repair newRepair)
        {
            var repair = await _context.Repairs.FirstOrDefaultAsync(x => x.Id == repairId);
            if (repair != null)
            {
                repair.Price = newRepair.Price;
                repair.Name = newRepair.Name;
                await _context.SaveChangesAsync();
            }
        }
    }
}
