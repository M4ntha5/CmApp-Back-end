using CmApp.Domains;
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IRepairService
    {
        Task<List<RepairEntity>> GetAllSelectedCarRepairs(string carId);
        Task DeleteSelectedCarRepair(string carId, string repairId);
        Task InsertCarRepairs(string carId, List<RepairEntity> repairs);
        Task<RepairEntity> GetSelectedCarRepairById(string carId, string repairid);
        Task UpdateSelectedCarRepair(string repairid, string carId, RepairEntity repair);
        Task DeleteAllCarRepairs(string carId);
    }
}
