using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CmApp.BusinessLogic.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly Context _context;

        public CarRepository(Context context)
        {
            _context = context;
        }

        public Task InsertCar(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car), "Cannot insert car in db, because car is empty");

            _context.Cars.Add(car);
            return _context.SaveChangesAsync();       
        }
        public IQueryable<Car> GetUserCars(int userId)
        {
            return _context.Cars
                .Include(x => x.Repairs)
                .Include(x => x.Make)
                .Include(x => x.Model)
                .Where(x => x.UserId == userId)
                .AsQueryable();
        }
        public async Task UpdateCarDefaultImage(int carId, string image)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            if (car == null)
                throw new BusinessException("Cannot update default car image, because such car not found");
            car.DefaultImage = image;
            await _context.SaveChangesAsync();
        }
        public Task DeleteCar(int carId)
        {
            var car = _context.Cars
                .Include(x => x.Repairs)
                .Include(x => x.Tracking).ThenInclude(x => x.Images)
                .Include(x => x.Equipment)
                .Include(x => x.Images)
                .FirstOrDefault(x => x.Id == carId);
            if (car == null)
                throw new BusinessException("Cannot delete car, because such car not found");
            
            _context.Cars.Remove(car);
            return _context.SaveChangesAsync();
        }

        public Task<List<Car>> GetAllCars()
        {
            return _context.Cars.ToListAsync();
        }

        public Task<List<Car>> GetAllUserCars(int userId)
        {
            return _context.Cars
                .Include(x => x.Make).ThenInclude(x => x.Models)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        public Car GetCarById(int carId)
        {
            return _context.Cars.FirstOrDefault(x => x.Id == carId);
        }

        public Task UpdateCar(int carId, Car newCar)
        {
            var car = _context.Cars.FirstOrDefault(x => x.Id == carId);
            if (car == null)
                throw new BusinessException("Cannot update default car image, because such car not found");
            
            car.Vin = newCar.Vin;
            car.ManufactureDate = newCar.ManufactureDate;
            car.Series = newCar.Series;
            car.BodyType = newCar.BodyType;
            car.Steering = newCar.Steering;
            car.FuelType = newCar.FuelType;
            car.Engine = newCar.Engine;
            car.Displacement = newCar.Displacement;
            car.Power = newCar.Power;
            car.Drive = newCar.Drive;
            car.Transmission = newCar.Transmission;
            car.Color = newCar.Color;
            car.Interior = newCar.Interior;
            car.MakeId = newCar.MakeId;
            car.ModelId = newCar.ModelId;
            car.UserId = newCar.UserId;
            car.DefaultImage = newCar.DefaultImage;
            car.IsSold = newCar.IsSold;
            car.SoldPrice = newCar.SoldPrice;
            car.SoldDate = newCar.SoldDate;
            car.CreatedAt = DateTime.Now;
            car.BoughtPrice = newCar.BoughtPrice;

            return _context.SaveChangesAsync();
        }

        public Task<List<CarStats>> GetCarStats(DateTime dateFrom, DateTime dateTo, string userEmail)
        {
            throw new NotImplementedException();
        }


        public bool CheckIfUserAlreadyHasCarWithSuchVin(int userId, string vin)
        {
            return _context.Cars
                .Any(x => x.UserId == userId 
                               && string.Equals(x.Vin, vin, StringComparison.CurrentCultureIgnoreCase));
        }


        public Task<List<string>> UploadImageToCar(int recordId, List<string> urls)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAllCarImages(int recordId)
        {
            throw new NotImplementedException();
        }
    }
}
