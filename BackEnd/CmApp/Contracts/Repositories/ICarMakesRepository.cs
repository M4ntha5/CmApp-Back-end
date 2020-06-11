using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarMakesRepository
    {
        Task<List<CarMakesEntity>> GetAllMakes();
        Task<CarMakesEntity> InsertCarMake(CarMakesEntity make);
        Task UpdateCarMake(CarMakesEntity make);
        Task DeleteCarMake(string makeId);
        Task<CarMakesEntity> GetMakeModels(string make);
    }
}
