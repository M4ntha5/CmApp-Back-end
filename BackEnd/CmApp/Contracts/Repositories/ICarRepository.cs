using CmApp.Domains;
using CmApp.Entities;
using Isidos.CodeMash.ServiceContracts;
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
        Task<UploadRecordFileResponse> UploadImageToCar(string recordId, byte[] bytes, string imgName);
        Task<List<CarDisplay>> GetAllUserCars(string userId);
        Task DeleteAllCarImages(string recordId);
        Task<List<CarMakes>> GetAllMakes();
        Task<CarMakes> InsertCarMake(CarMakes make);
        Task UpdateCarMake(CarMakes make);
        Task DeleteCarMake(string makeId);
    }
}
