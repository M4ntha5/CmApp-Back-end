using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface ICarService
    {
        Task<CarEntity> InsertCar(CarEntity car);
        Task DeleteCar(int userId, int carId);
        Task UpdateCar(int userId, int carId, CarEntity car);
        Task<List<string>> InsertImages(int carId, List<string> images);
        Task DeleteImages(int carId, List<string> images);
        Task UpdateImages(int carId, Images images);
    }
}
