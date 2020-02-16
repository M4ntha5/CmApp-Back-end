using CmApp.Entities;
using Isidos.CodeMash.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarRepository
    {
        Task<CarEntity> InsertCar(CarEntity car);
        Task<List<CarEntity>> GetAllCars();
        Task UpdateCar(string id, CarEntity car);
        Task DeleteCar(CarEntity car);
        Task<CarEntity> GetCarById(string id);
        Task<UploadFileResponse> UploadImage(string vin, string fileName);
        Task AddImageToCar(string carId, Image img);
    }
}
