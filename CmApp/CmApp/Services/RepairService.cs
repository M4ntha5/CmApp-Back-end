using CmApp.Contracts;
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class RepairService : IRepairService
    {
        public IRepairRepository RepairRepository { get; set; }

        public async Task DeleteRepair(string repairId)
        {
            await RepairRepository.DeleteRepair(repairId);
        }

        public async Task<List<RepairEntity>> GetAllCarRepairs(string carId)
        {
            var repairs = await RepairRepository.GetAllRepairsByCarId(carId);
            return repairs;
        }
    }
}
