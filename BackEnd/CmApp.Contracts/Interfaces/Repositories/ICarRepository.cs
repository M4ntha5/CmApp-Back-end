using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface ICarRepository
    {
        Task<CarEntity> InsertCar(CarEntity car);
        Task<List<CarEntity>> GetAllCars();
        Task UpdateCar(int carId, CarEntity car);
        Task DeleteCar(int carId);
        Task<CarEntity> GetCarById(int carId);
        Task<List<string>> UploadImageToCar(int recordId, List<string> urls);
        Task<List<CarDisplay>> GetAllUserCars(int userId);
        Task DeleteAllCarImages(int recordId);
    }
}
