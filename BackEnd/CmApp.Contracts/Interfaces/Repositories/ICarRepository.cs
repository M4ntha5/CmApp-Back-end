using CmApp.Contracts.DTO;
using CmApp.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface ICarRepository
    {
        bool CheckIfUserAlreadyHasCarWithSuchVin(int userId, string vin);
        IQueryable<Car> GetUserCars(int userId);
        Task InsertCar(Car car);
        Task UpdateCarDefaultImage(int carId, string image);


        //Task<Car> InsertCar(Car car);
        Task<List<Car>> GetAllCars();
        Task UpdateCar(int carId, Car car);
        Task DeleteCar(int carId);
        Car GetCarById(int carId);
        Task<List<Car>> GetAllUserCars(int userId);
        Task<List<CarStats>> GetCarStats(DateTime dateFrom, DateTime dateTo, string userEmail);
        


        Task<List<string>> UploadImageToCar(int recordId, List<string> urls);
        Task DeleteAllCarImages(int recordId);
    }
}
