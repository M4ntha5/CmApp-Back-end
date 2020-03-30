using CmApp.Domains;
using CmApp.Entities;
using CodeMash.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IRepairRepository
    {
        Task<DatabaseDeleteOneResponse> DeleteRepair(string carId, string repairId);
        Task UpdateRepair(string repairId, RepairEntity repair);
        Task<RepairEntity> GetCarRepairById(string carId, string repairId);
        Task<RepairEntity> InsertRepair(RepairEntity repair);
        Task<List<RepairEntity>> GetAllRepairsByCarId(string carId);
        Task DeleteMultipleRepairs(string carId);
        Task InsertMultipleRepairs(List<RepairEntity> repairs);
    }
}
