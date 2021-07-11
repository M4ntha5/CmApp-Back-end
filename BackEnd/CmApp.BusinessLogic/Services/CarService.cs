using AutoMapper;
using CmApp.Contracts.DTO;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using CmApp.Utils;
using image4ioDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CmApp.BusinessLogic.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        
        private readonly IScraperService _webScraper;
        private readonly IFileRepository _fileRepository;

        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IScraperService webScraper,
            IFileRepository fileRepository, IMapper mapper, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _carRepository = carRepository;
            _webScraper = webScraper;
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public Task<List<CarListDTO>> GetUserCars(int userId)
        {
            var cars = _carRepository.GetUserCars(userId);
            return cars
                .Select(x => new CarListDTO 
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

        public Task InsertCar(int userId, CarDTO car)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                throw new BusinessException("Cannot add car because such user not found");
            if (_carRepository.CheckIfUserAlreadyHasCarWithSuchVin(userId, car.Vin))
                throw new BusinessException("You already have a car with such VIN number");

            //var carEntity = _mapper.Map<Car>(car);
            var carEntity = new Car
            {
                Vin = car.Vin.ToUpper(),
                UserId = userId,
                MakeId = car.MakeId,
                ModelId = car.ModelId,
                BoughtPrice = car.BoughtPrice,
                DefaultImage = Settings.DefaultImageUrl,
                ManufactureDate = car.ManufactureDate,
                Series = car.Series,
                BodyType = car.BodyType,
                Steering = car.Steering,
                FuelType = car.FuelType,
                Engine = car.Engine,
                Displacement = car.Displacement,
                Power = car.Power,
                Drive = car.Drive,
                Transmission = car.Transmission,
                Color = car.Color,
                Interior = car.Interior,
                CreatedAt = DateTime.Now,
            };
            return _carRepository.InsertCar(carEntity);
            
            
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
        
        public Task DeleteCar(int userId, int carId)
        {
            return _carRepository.DeleteCar(carId);
            
            //await FileRepository.DeleteFolder("/cars/" + carId);

            //await FileRepository.DeleteFolder("/tracking/" + tracking.Id);
        }

        public Task UpdateCar(int userId, int carId, Car car)
        {
            // if (car.User != userId)
            //     throw new BusinessException("Car does not exist");

            /*var list = car.Equipment.Select(x => x.Code).ToList().Distinct().Count();
            if (car.Equipment.Count != list)
                throw new BusinessException("Car cannot have multiple equipment with the same code!");
            */
            return _carRepository.UpdateCar(carId, car);
        }

        public async Task<List<string>> InsertImages(int carId, List<string> images)
        {
            if (images != null && images.Count > 0)
            {
                //deletes form cloud
                await _fileRepository.DeleteFolder("/cars/" + carId);
                //deletes from db
                await _carRepository.DeleteAllCarImages(carId);

                var imgsList = new List<UploadImageRequest.File>();

                int count = 1;
                images.ForEach(x => imgsList.Add(
                    new UploadImageRequest.File()
                    {
                        FileName = count++ + ".jpeg",
                        Data = new MemoryStream(_fileRepository.Base64ToByteArray(x.Split(',')[1]))
                    }
                ));

                //inserts to cloud 
                var insertedUrls = await _fileRepository.InsertCarImages(carId, imgsList);
                //inserts to db
                await _carRepository.UploadImageToCar(carId, insertedUrls);
                return insertedUrls;
            }
            else
            {
                //deletes form cloud
                await _fileRepository.DeleteFolder("/cars/" + carId);
                //deletes from db
                await _carRepository.DeleteAllCarImages(carId);
                return null;
            }
        }

        public async Task DeleteImages(int carId, List<string> images)
        {
            if (images != null && images.Count > 0)
            {
                foreach (var img in images)
                {
                    var path = img.Split("cmapp")[1];
                    await _fileRepository.DeleteImage(path);
                }
            }
        }

        public async Task UpdateImages(int carId, Images images)
        {
            //deleting images from cloud
            await DeleteImages(carId, images.Deleted);

            List<string> urls = new List<string>();
            List<string> base64 = new List<string>();
            //images.All.ForEach(x =>
            //{
            //    if (x.Url.StartsWith("http"))
            //        urls.Add(x.Url);
            //    else
            //        base64.Add(x.Url);
            //});

            if (base64.Count > 0)
            {
                var imgsList = new List<UploadImageRequest.File>();
                int count = 1;
                base64.ForEach(x => imgsList.Add(
                    new UploadImageRequest.File()
                    {
                        FileName = count++ + ".jpeg",
                        Data = new MemoryStream(_fileRepository.Base64ToByteArray(x.Split(',')[1]))
                    }
                ));

                //inserts to cloud 
                var insertedUrls = await _fileRepository.InsertCarImages(carId, imgsList);
                if (insertedUrls != null)
                {
                    insertedUrls.ForEach(x => urls.Add(x));
                }
            }
            await _carRepository.UploadImageToCar(carId, urls);
        }
    }
}
