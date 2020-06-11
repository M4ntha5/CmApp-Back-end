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
        public ISummaryRepository SummaryRepository { get; set; }

        public async Task<List<RepairEntity>> GetAllSelectedCarRepairs(string carId)
        {
            var repairs = await RepairRepository.GetAllRepairsByCarId(carId);

            if (repairs != null && repairs.Count > 0)
                repairs[0].Total = repairs.Sum(x => x.Price);

            return repairs;
        }
        public async Task InsertCarRepairs(string carId, List<RepairEntity> repairs)
        {
            repairs.ForEach(x => { x.Car = carId; Math.Round(x.Price, 2); Math.Round(x.Total, 2); });
            var repairsTotal = repairs.Sum(x => x.Price);

            await RepairRepository.InsertMultipleRepairs(repairs);
            var summary = await SummaryRepository.GetSummaryByCarId(carId);

            var total = Math.Round(summary.Total + repairsTotal, 2);
            await SummaryRepository.InsertTotalByCar(summary.Id, total);
        }
        public async Task<RepairEntity> GetSelectedCarRepairById(string carId, string repairid)
        {
            var response = await RepairRepository.GetCarRepairById(carId, repairid);
            if (response == null)
                throw new BusinessException("Repair does not exists!");

            return response;
        }
        public async Task DeleteMultipleRepairs(string carId)
        {
            var summary = await SummaryRepository.GetSummaryByCarId(carId);
            var repairs = await RepairRepository.GetAllRepairsByCarId(carId);
            var repairsTotal = repairs.Sum(x => x.Price);
            await SummaryRepository.InsertTotalByCar(summary.Id, summary.Total - repairsTotal);
            await RepairRepository.DeleteMultipleRepairs(carId);
        }
    }
}
