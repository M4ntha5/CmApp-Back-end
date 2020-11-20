using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface ICarMakesRepository
    {
        Task<List<Make>> GetAllMakes();
        Task<Make> InsertCarMake(Make make);
        Task UpdateCarMake(Make make);
        Task DeleteCarMake(int makeId);
        Task<Make> GetMakeModels(string make);
    }
}
