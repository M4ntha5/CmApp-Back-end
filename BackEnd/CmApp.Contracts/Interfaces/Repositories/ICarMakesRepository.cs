using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface ICarMakesRepository
    {
        Task<List<CarMakesEntity>> GetAllMakes();
        Task<CarMakesEntity> InsertCarMake(CarMakesEntity make);
        Task UpdateCarMake(CarMakesEntity make);
        Task DeleteCarMake(int makeId);
        Task<CarMakesEntity> GetMakeModels(string make);
    }
}
