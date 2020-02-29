using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ICarService
    {
        Task<CarEntity> InsertCarDetailsFromScraper(CarEntity car);
        Task DeleteCar(string id);
        Task<CarEntity> GetCarById(string id);
        Task<List<CarEntity>> GetAllCars();
        Task UpdateCar(string id, CarEntity car);


    }
}
