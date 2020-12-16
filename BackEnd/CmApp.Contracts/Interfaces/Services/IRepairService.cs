using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IRepairService
    {
        Task<List<Repair>> GetAllSelectedCarRepairs(int carId);
        Task InsertCarRepairs(int carId, List<Repair> repairs);
        Task<Repair> GetSelectedCarRepairById(int carId, int repairid);
        Task DeleteMultipleRepairs(int carId);
    }
}
