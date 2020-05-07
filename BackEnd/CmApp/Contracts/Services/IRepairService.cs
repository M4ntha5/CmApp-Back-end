
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IRepairService
    {
        Task<List<RepairEntity>> GetAllSelectedCarRepairs(string carId);
        Task InsertCarRepairs(string carId, List<RepairEntity> repairs);
        Task<RepairEntity> GetSelectedCarRepairById(string carId, string repairid);
    }
}
