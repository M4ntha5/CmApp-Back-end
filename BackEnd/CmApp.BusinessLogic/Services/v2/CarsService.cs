using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CmApp.BusinessLogic.Repositories;
using CmApp.Contracts.DTO;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using CmApp.Utils;
using Microsoft.EntityFrameworkCore;

namespace CmApp.BusinessLogic.Services.v2
{
    public class CarsService : ICarsService
    {
        private readonly IScraperService _scraperService;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public CarsService(Context context, IScraperService webScraper, IMapper mapper)
        {
            _scraperService = webScraper;
            _mapper = mapper;
            _context = context;
        }

        public async Task InsertCar(int userId, CarDTO car)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new BusinessException("Cannot add car because such user not found");
            if (await _context.Cars.AnyAsync(x => x.Vin == car.Vin && x.UserId == userId))
                throw new BusinessException("You already have a car with such VIN number");

            //var carEntity = _mapper.Map<Car>(car);
            var carEntity = new Car
            {
                CreatedAt = DateTime.Now,
                Vin = car.Vin.ToUpper(),
                UserId = userId,
                //!!!!!! temporary !!!!!!!!!!!!!!
                DefaultImage = Settings.DefaultImageUrl,
                BodyType = car.BodyType,
                Color = car.Color,
                Displacement = car.Displacement,
                Drive = car.Drive,
                Engine = car.Engine,
                FuelType = car.FuelType,
                Interior = car.Interior,
                MakeId = car.MakeId,
                ManufactureDate = car.ManufactureDate,
                ModelId = car.ModelId,
                Transmission = car.Transmission,
                Power = car.Power,
                Series = car.Series,
                Steering = car.Steering
            };

            //inserting vehicle data
            _context.Cars.Add(carEntity);
            await _context.SaveChangesAsync();
            //inserting images

            /*var insertedImages = await InsertImages(carEntity.Id, car.Images);

            //setting default image to the first one of a list
            if (insertedImages.Count < 1)
                carEntity.DefaultImage = Settings.DefaultImageUrl;
            else
                carEntity.DefaultImage = car.Images.FirstOrDefault();

            await CarRepository.UpdateCarDefaultImage(carEntity.Id, carEntity.DefaultImage);*/

            //inserts empty tracking 
        }

        public async Task DeleteCar(int userId, int carId)
        {
            var car = await _context.Cars
                .Include(x => x.Repairs)
                .Include(x => x.Images)
                .Include(x => x.Equipment)
                .Include(x => x.Tracking).ThenInclude(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == carId && x.UserId == userId); 
            if (car == null)
                throw new BusinessException("Car does not exist");

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            //await FileRepository.DeleteFolder("/cars/" + car.Id);
            //await FileRepository.DeleteFolder("/tracking/" + car.Tracking?.Id);
        }

        public async Task UpdateCar(int userId, int carId, CarDTO car)
        {
            var oldCarData = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            if(oldCarData == null)
                throw new BusinessException("Cannot update car, because such car not found");
            
            //oldCarData.Vin = car.Vin;
            oldCarData.ManufactureDate = car.ManufactureDate;
            oldCarData.Series = car.Series;
            oldCarData.BodyType = car.BodyType;
            oldCarData.Steering = car.Steering;
            oldCarData.FuelType = car.FuelType;
            oldCarData.Engine = car.Engine;
            oldCarData.Displacement = car.Displacement;
            oldCarData.Power = car.Power;
            oldCarData.Drive = car.Drive;
            oldCarData.Transmission = car.Transmission;
            oldCarData.Color = car.Color;
            oldCarData.Interior = car.Interior;
            oldCarData.MakeId = car.MakeId;
            oldCarData.CreatedAt = DateTime.Now;
            oldCarData.BoughtPrice = car.BoughtPrice;
            
            await _context.SaveChangesAsync();
        }

        public Task<List<CarListDTO>> GetUserCars(int userId)
        {
            return _context.Cars
                .Include(x => x.Repairs)
                .Where(x => x.UserId == userId)
                .Select(x => new CarListDTO()
                {
                    Id = x.Id,
                    Make = x.Make.Name,
                    Model = x.Model.Name,
                    Vin = x.Vin,
                    DefaultImage = x.DefaultImage,
                    Total = GetCarTotalSpent(x),
                    Sold = x.IsSold,
                    Profit = GetCarProfit(x),
                    SoldWithin = GetSoldWithin(x),
                })
                .ToListAsync();
        }
        private static decimal GetCarTotalSpent(Car car)
        {
            var repairsTotal = car.Repairs.Count > 0 ? car.Repairs.Sum(x => x.Price) : 0;
            return repairsTotal + car.BoughtPrice;
        }
        private static decimal? GetCarProfit(Car car)
        {
            if (!car.IsSold || car.SoldPrice == null)
                return null;
            var totalSpent = GetCarTotalSpent(car);
            var profit = car.SoldPrice - totalSpent;

            return profit;
        }
        private static string GetSoldWithin(Car car)
        {
            if (!car.IsSold || car.SoldDate == null)
                return default;
            var time = car.SoldDate.Value.Subtract(car.CreatedAt);
            string message;
            if (time.Days > 0)
                message = time.Days == 1 ? $"Sold within {time.Days} day" : $"Sold within {time.Days} days";
            else
                message = time.Hours == 1 ? $"Sold within {time.Hours} hour" : $"Sold within {time.Hours} hours";
            
            return message;
        }
    }
}