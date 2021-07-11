using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmApp.Contracts.DTO.v2;

namespace CmApp.BusinessLogic.Repositories
{
    public class RepairRepository : IRepairRepository
    {
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
            return _context.Repairs.Where(x => x.CarId == carId && !x.IsShipping).ToListAsync();
        }
        public Task<List<Repair>> GetAllShippingByCarId(int carId)
        {
            return _context.Repairs.Where(x => x.CarId == carId && x.IsShipping).ToListAsync();
        }

        public Task<Repair> GetCarRepairById(int carId, int repairId)
        {
            return _context.Repairs.FirstOrDefaultAsync(x => x.Id == repairId && x.CarId == carId);
        }

        public Task InsertMultipleRepairs(int carId, List<RepairDTO> repairs)
        {
            if (repairs == null || repairs.Count < 1)
                throw new ArgumentNullException(nameof(repairs), "Cannot insert repairs in db, because repairs is empty");

            var mappedRepairs = repairs
                .Select(repair => new Repair()
                {
                    CarId = carId, 
                    IsShipping = repair.IsShipping,
                    Name = repair.Name, 
                    Price = repair.Price
                }).ToList();

            return _context.Repairs.AddRangeAsync(mappedRepairs);
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
