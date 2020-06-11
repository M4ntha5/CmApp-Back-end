using CmApp.Domains;
using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarRepository
    {
        Task<CarEntity> InsertCar(CarEntity car);
        Task<List<CarEntity>> GetAllCars();
        Task UpdateCar(string carId, CarEntity car);
        Task DeleteCar(string carId);
        Task<CarEntity> GetCarById(string carId);
        Task<List<string>> UploadImageToCar(string recordId, List<string> urls);
        Task<List<CarDisplay>> GetAllUserCars(string userId);
        Task DeleteAllCarImages(string recordId);
    }
}
