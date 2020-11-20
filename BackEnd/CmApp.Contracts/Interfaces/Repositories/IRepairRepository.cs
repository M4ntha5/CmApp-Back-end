using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IRepairRepository
    {
        Task UpdateRepair(int repairId, Repair repair);
        Task<Repair> GetCarRepairById(int carId, int repairId);
        Task<Repair> InsertRepair(Repair repair);
        Task<List<Repair>> GetAllRepairsByCarId(int carId);
        Task DeleteMultipleRepairs(int carId);
        Task InsertMultipleRepairs(List<Repair> repairs);
    }
}
