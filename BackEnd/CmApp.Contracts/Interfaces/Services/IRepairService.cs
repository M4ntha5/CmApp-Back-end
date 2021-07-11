using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CmApp.Contracts.DTO.v2;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IRepairService
    {
        Task<List<Repair>> GetAllSelectedCarRepairs(int carId);
        Task InsertCarRepairs(int carId, List<RepairDTO> repairs);
        Task<Repair> GetSelectedCarRepairById(int carId, int repairId);
        Task DeleteMultipleRepairs(int carId);
        Task<List<Repair>> GetAllSelectedCarShipping(int carId);
    }
}
