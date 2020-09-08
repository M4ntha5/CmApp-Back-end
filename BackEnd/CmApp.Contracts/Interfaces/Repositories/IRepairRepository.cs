using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IRepairRepository
    {
        Task UpdateRepair(int repairId, RepairEntity repair);
        Task<RepairEntity> GetCarRepairById(int carId, int repairId);
        Task<RepairEntity> InsertRepair(RepairEntity repair);
        Task<List<RepairEntity>> GetAllRepairsByCarId(int carId);
        Task DeleteMultipleRepairs(int carId);
        Task InsertMultipleRepairs(List<RepairEntity> repairs);
    }
}
