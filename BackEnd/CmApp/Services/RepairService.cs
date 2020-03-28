using CmApp.Contracts;
using CmApp.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class RepairService : IRepairService
    {
        public IRepairRepository RepairRepository { get; set; }
        public ISummaryRepository SummaryRepository { get; set; }

        public async Task DeleteSelectedCarRepair(string carId, string repairId)
        {
            await RepairRepository.DeleteRepair(carId, repairId);
        }

        public async Task<List<RepairEntity>> GetAllSelectedCarRepairs(string carId)
        {
            var repairs = await RepairRepository.GetAllRepairsByCarId(carId);

            if (repairs != null && repairs.Count > 0)
                repairs[0].Total = repairs.Sum(x => x.Price);

            return repairs;
        }
        public async Task<RepairEntity> InsertCarRepair(string carId, RepairEntity repair)
        {
            repair.Car = carId;
            var response = await RepairRepository.InsertRepair(repair);
            var summary = await SummaryRepository.GetSummaryByCarId(carId);

            await SummaryRepository.InsertTotalByCar(summary.Id, summary.Total + repair.Price);
            return response;
        }
        public async Task<RepairEntity> GetSelectedCarRepairById(string carId, string repairid)
        {
            var response = await RepairRepository.GetCarRepairById(carId, repairid);
            if (response == null)
                throw new BusinessException("Repair does not exists!");

            return response;
        }
        public async Task UpdateSelectedCarRepair(string repairid, string carId, RepairEntity repair)
        {
            repair.Car = carId;
            await RepairRepository.UpdateRepair(repairid, repair);
        }

    }
}
