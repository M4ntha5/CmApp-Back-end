using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IMakeRepository
    {
        //v1
        Task<List<Make>> GetAllMakes();
        Task<Make> InsertCarMake(Make make);
        Task UpdateCarMake(Make make);
        Task DeleteCarMake(int makeId);
        Task<Make> GetMakeModels(string make);

        //v2
        Task<List<Make>> GetMakes();
        Task<Make> GetMake(int makeId);
        Task InsertMake(MakeDTO make);
        Task DeleteMake(int makeId);
        Task UpdateMake(int makeId, MakeDTO newMake);
    }
}
