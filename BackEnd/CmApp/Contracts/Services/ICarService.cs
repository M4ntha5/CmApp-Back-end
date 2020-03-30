using CmApp.Domains;
using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarService
    {
        Task<CarEntity> InsertCar(CarEntity car);
        Task DeleteCar(string userId, string carId);
        Task<CarEntity> GetCarById(string carId);
        Task<List<CarEntity>> GetAllCars();
        Task UpdateCar(string userId, string carId, CarEntity car);
        Task<List<CarDisplay>> GetAllUserCars(string userid);

    }
}
