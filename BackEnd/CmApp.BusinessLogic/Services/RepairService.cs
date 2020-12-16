using CmApp.Contracts;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class RepairService : IRepairService
    {
        private readonly IRepairRepository RepairRepository;
        private readonly ISummaryRepository SummaryRepository;

        public RepairService(IRepairRepository repairRepository, ISummaryRepository summaryRepository)
        {
            RepairRepository = repairRepository;
            SummaryRepository = summaryRepository;
        }

        public Task DeleteMultipleRepairs(int carId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Repair>> GetAllSelectedCarRepairs(int carId)
        {
            throw new NotImplementedException();
        }

        public Task<Repair> GetSelectedCarRepairById(int carId, int repairid)
        {
            throw new NotImplementedException();
        }

        public Task InsertCarRepairs(int carId, List<Repair> repairs)
        {
            throw new NotImplementedException();
        }

        /*public async Task<List<Repair>> GetAllSelectedCarRepairs(int carId)
        {
            var repairs = await RepairRepository.GetAllRepairsByCarId(carId);

            if (repairs != null && repairs.Count > 0)
                repairs[0].Total = repairs.Sum(x => x.Price);

            return repairs;
        }
        public async Task InsertCarRepairs(int carId, List<Repair> repairs)
        {
            repairs.ForEach(x => { x.Car = carId; Math.Round(x.Price, 2); Math.Round(x.Total, 2); });
            var repairsTotal = repairs.Sum(x => x.Price);

            await RepairRepository.InsertMultipleRepairs(repairs);
            var summary = await SummaryRepository.GetSummaryByCarId(carId);

            var total = Math.Round(summary.Total + repairsTotal, 2);
            await SummaryRepository.InsertTotalByCar(summary.ID, total);
        }
        public async Task<Repair> GetSelectedCarRepairById(int carId, int repairid)
        {
            var response = await RepairRepository.GetCarRepairById(carId, repairid);
            if (response == null)
                throw new BusinessException("Repair does not exists!");

            return response;
        }
        public async Task DeleteMultipleRepairs(int carId)
        {
            var summary = await SummaryRepository.GetSummaryByCarId(carId);
            var repairs = await RepairRepository.GetAllRepairsByCarId(carId);
            var repairsTotal = repairs.Sum(x => x.Price);
            await SummaryRepository.InsertTotalByCar(summary.ID, summary.Total - repairsTotal);
            await RepairRepository.DeleteMultipleRepairs(carId);
        }*/
    }
}
