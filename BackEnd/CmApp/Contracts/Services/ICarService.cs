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
        Task UpdateCar(string userId, string carId, CarEntity car);
        Task<List<string>> InsertImages(string carId, List<string> images);
        Task DeleteImages(string carId, List<string> images);
        Task UpdateImages(string carId, Images images);


        Task InsertEquipment(string carId, List<Equipment> data);
    }
}
