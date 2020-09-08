using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IRepairService
    {
        Task<List<RepairEntity>> GetAllSelectedCarRepairs(int carId);
        Task InsertCarRepairs(int carId, List<RepairEntity> repairs);
        Task<RepairEntity> GetSelectedCarRepairById(int carId, int repairid);
        Task DeleteMultipleRepairs(int carId);
    }
}
