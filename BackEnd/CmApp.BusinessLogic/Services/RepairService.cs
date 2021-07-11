using CmApp.Contracts;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CmApp.Contracts.DTO.v2;

namespace CmApp.BusinessLogic.Services
{
    public class RepairService : IRepairService
    {
        private readonly IRepairRepository _repairRepository;
        private readonly ICarRepository _carRepository;

        public RepairService(IRepairRepository repairRepository, ICarRepository carRepository)
        {
            _repairRepository = repairRepository;
            _carRepository = carRepository;
        }

        public Task<List<Repair>> GetAllSelectedCarRepairs(int carId)
        {
            return _repairRepository.GetAllRepairsByCarId(carId);
        }
        public Task<List<Repair>> GetAllSelectedCarShipping(int carId)
        {
            return _repairRepository.GetAllRepairsByCarId(carId);
        }
        public Task InsertCarRepairs(int carId, List<RepairDTO> repairs)
        {
            var car = _carRepository.GetCarById(carId);
            if (car == null)
                throw new BusinessException("Cannot add repair, because car not found");
            return _repairRepository.InsertMultipleRepairs(carId, repairs);
        }
        public Task<Repair> GetSelectedCarRepairById(int carId, int repairId)
        {
            return _repairRepository.GetCarRepairById(carId, repairId);
        }
        public async Task DeleteMultipleRepairs(int carId)
        {
            var repairs = await _repairRepository.GetAllRepairsByCarId(carId);
            var repairsTotal = repairs.Sum(x => x.Price);
            //await SummaryRepository.InsertTotalByCar(summary.ID, summary.Total - repairsTotal);
            await _repairRepository.DeleteMultipleRepairs(carId);
        }
    }
}
