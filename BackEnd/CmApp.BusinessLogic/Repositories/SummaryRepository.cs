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
        /*private readonly Context _context;

        public SummaryRepository(Context context)
        {
            _context = context;
        }

        public async Task InsertSummary(Summary summary)
        {
            if (summary == null)
                throw new ArgumentNullException(nameof(summary), "Cannot insert summary in db, because summary is empty");

            await _context.Summaries.AddAsync(summary);
            await _context.SaveChangesAsync();
        }*/
        

       /* public async Task DeleteCarSummary(int carId)
        {
            var summary = await _context.Summaries.FirstOrDefaultAsync(x => x.CarId == carId);
            if (summary != null)
            {
                _context.Summaries.Remove(summary);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Summary> GetSummaryByCarId(int carId)
        {
            return null;// return _context.Summaries.FirstOrDefaultAsync(x => x.CarId == carId);
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
        }*/
    }
}
