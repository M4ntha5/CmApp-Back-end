using CmApp.Contracts.DTO;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface ICarService
    {
        Task<Car> InsertCar(int userId, CarDTO car);
        Task DeleteCar(int userId, int carId);
        Task UpdateCar(int userId, int carId, Car car);
        Task<List<string>> InsertImages(int carId, List<string> images);
        Task DeleteImages(int carId, List<string> images);
        Task UpdateImages(int carId, Images images);

        Task<List<CarListDTO>> GetUserCars(int userId);
    }
}
