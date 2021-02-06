using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Repositories
{
    public class SummaryRepository : ISummaryRepository
    {
        /*  private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

          public async Task<SummaryEntity> InsertSummary(SummaryEntity summary)
          {
              if (summary == null)
                  throw new ArgumentNullException(nameof(summary), "Cannot insert summary in db, because summary is empty");

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
              var filter = Builders<SummaryEntity>.Filter.Eq("car", BsonObjectId.Create(carId));
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
              var update = Builders<SummaryEntity>.Update.Set("total", total);

              await repo.UpdateOneAsync(
                  x => x.Id == summaryId,
                  update,
                  new DatabaseUpdateOneOptions()
              );
          }*/

        private readonly Context _context;

        public SummaryRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteCarSummary(int carId)
        {
           /* var summary = await _context.Summaries.FirstOrDefaultAsync(x => x.CarId == carId);
            if (summary != null)
            {
                _context.Summaries.Remove(summary);
                await _context.SaveChangesAsync();
            }*/
        }

        public Task<Summary> GetSummaryByCarId(int carId)
        {
            return null;// return _context.Summaries.FirstOrDefaultAsync(x => x.CarId == carId);
        }

        public async Task<Summary> InsertSummary(Summary summary)
        {
            if (summary == null)
                throw new ArgumentNullException(nameof(summary), "Cannot insert summary in db, because summary is empty");

            await _context.Summaries.AddAsync(summary);
            await _context.SaveChangesAsync();
            return summary;
        }

        public async Task UpdateCarSoldSummary(int summaryId, Summary summaryDetails)
        {
            var summary = await _context.Summaries.FirstOrDefaultAsync(x => x.Id == summaryId);
            if(summary != null)
            {
                summary.IsSold = true;
                summary.SoldPrice = summaryDetails.SoldPrice;
                summary.SoldDate = DateTime.Today;
                await _context.SaveChangesAsync();
            }    
        }
    }
}
