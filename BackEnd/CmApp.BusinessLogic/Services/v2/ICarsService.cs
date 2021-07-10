using System.Collections.Generic;
using System.Threading.Tasks;
using CmApp.Contracts.DTO;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Models;

namespace CmApp.BusinessLogic.Services.v2
{
    public interface ICarsService
    {
        Task InsertCar(int userId, CarDTO car);
        Task DeleteCar(int userId, int carId);
        Task UpdateCar(int userId, int carId, CarDTO car);
        Task<List<CarListDTO>> GetUserCars(int userId);
        
        //Task<List<string>> InsertImages(int carId, List<string> images);
        //Task DeleteImages(int carId, List<string> images);
        //Task UpdateImages(int carId, Images images);
    }
}