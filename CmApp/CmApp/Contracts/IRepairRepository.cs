using CmApp.Domains;
using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IRepairRepository
    {
        Task DeleteRepair(string repairId);
        Task UpdateRepair(string repairId, Repair repair);
        Task<RepairEntity> GetRepairById(string repairId);
        Task<List<RepairEntity>> GetAllRepairs();
        Task<RepairEntity> InsertRepair(Repair repair);
        Task<List<RepairEntity>> GetAllRepairsByCarId(string carId);
    }
}
