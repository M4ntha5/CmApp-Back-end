using CmApp.Contracts.Domains;
using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface ICarRepository
    {
        Task<Car> InsertCar(Car car);
        Task<List<Car>> GetAllCars();
        Task UpdateCar(int carId, Car car);
        Task DeleteCar(int carId);
        Task<Car> GetCarById(int carId);
        Task<List<Car>> GetAllUserCars(int userId);


        Task<List<string>> UploadImageToCar(int recordId, List<string> urls);
        Task DeleteAllCarImages(int recordId);
    }
}
