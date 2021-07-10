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

namespace CmApp.BusinessLogic.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository CarRepository;
        private readonly ISummaryRepository SummaryRepository;
        private readonly ISummaryService SummaryService;
        private readonly IUserRepository UserRepository;

        private readonly IShippingRepository ShippingRepository;
        private readonly IScraperService WebScraper;
        private readonly IFileRepository FileRepository;
        private readonly ITrackingRepository TrackingRepository;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IScraperService webScraper, 
            ISummaryRepository summaryRepository, IShippingRepository shippingRepository, 
            IFileRepository fileRepository, ITrackingRepository trackingRepository, IMapper mapper,
            ISummaryService summaryService, IUserRepository userRepository)
        {
            SummaryService = summaryService;
            UserRepository = userRepository;
            CarRepository = carRepository;
            SummaryRepository = summaryRepository;
            WebScraper = webScraper;
            FileRepository = fileRepository;
            TrackingRepository = trackingRepository;
            ShippingRepository = shippingRepository;
            _mapper = mapper;
        }   

        private async Task<Car> InsertCarDetailsFromScraper(Car car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new BusinessException("Vin number cannot be null or empty!");
            if (car.Make.Name != "BMW" && car.Make.Name != "Mercedes-benz")
                throw new BusinessException("Bad vin for this make!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin, car.Make.Name);

            Car carEntity = new Car
            {
                Make = car.Make,                //default for this scraper
            };

            foreach (var param in parResults)
            {
                if (param.Key == "Prod. Date" || param.Key == "Production Date")
                    carEntity.ManufactureDate = Convert.ToDateTime(param.Value);
                /*else if (param.Key == "Type" || param.Key == "Model")
                    carEntity.Make.Name = param.Value;*/
                else if (param.Key == "Series")
                    carEntity.Series = param.Value;
                else if (param.Key == "Body Type")
                    carEntity.BodyType = param.Value;
                else if (param.Key == "Steering")
                    carEntity.Steering = param.Value;
                else if (param.Key == "Engine")
                    carEntity.Engine = param.Value;
                else if (param.Key == "Displacement")
                    carEntity.Displacement = decimal.Parse(param.Value);
                else if (param.Key == "Power")
                    carEntity.Power = param.Value;
                else if (param.Key == "Drive")
                    carEntity.Drive = param.Value;
                else if (param.Key == "Transmission")
                    carEntity.Transmission = param.Value;
                else if (param.Key == "Colour")
                    carEntity.Color = param.Value;
                else if (param.Key == "Upholstery")
                    carEntity.Interior = param.Value;
            }
            if (carEntity.Drive == "HECK")
                carEntity.Drive = "Rear wheel drive";
            else if (carEntity.Drive == "ALLR")
                carEntity.Drive = "All wheel drive";
            //making first letter upper
            if (carEntity.BodyType.ToLower().Contains("Coup".ToLower()))
                carEntity.BodyType = "Coupe";
            else if (carEntity.BodyType == "SAV")
                carEntity.BodyType = "SUV";
            else if (carEntity.BodyType == "LIM")
                carEntity.BodyType = "Limousine";

            var eqResults = WebScraper.GetVehicleEquipment(car.Vin, car.Make.Name);
            var equipment = new List<Contracts.Models.Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Contracts.Models.Equipment() { Code = eq.Key, Name = eq.Value });

            carEntity.Equipment = equipment;
            carEntity.Vin = car.Vin.ToUpper();

            //inserting vehicle data
            await CarRepository.InsertCar(carEntity);

            //await InsertImages(insertedCar.Id, car.Base64images);

            //inserts empty tracking 
            //await TrackingRepository.InsertTracking(new Tracking { Car = insertedCar });
            return null;

        }

        public async Task<List<CarListDTO>> GetUserCars(int userId)
        {
            var cars = await CarRepository.GetUserCars(userId);

            return cars.Select(x => new CarListDTO
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
            }).ToList();
        }
        private static decimal GetCarTotalSpent(Car car)
        {
            var repairsCost = car.Repairs != null ? car.Repairs.Sum(x => x.Price) : 0;
            var shippingCostSum =
                car.Shipping.AuctionFee + car.Shipping.Customs +
                car.Shipping.TransferFee + car.Shipping.TransportationFee;
            var shippingCost = shippingCostSum ?? 0;
            var totalSpent = car.Summary.BoughtPrice + repairsCost + shippingCost;
            return totalSpent;
        }

        private static decimal GetCarProfit(Car car)
        {
            if (!car.Summary.IsSold)
                return default;
            var totalSpent = GetCarTotalSpent(car);
            var profit = car.Summary.SoldPrice.Value - totalSpent;

            return profit;
        }
        private static string GetSoldWithin(Car car)
        {
            if (!car.Summary.IsSold)
                return default;
            var time = car.Summary.SoldDate.Value.Subtract(car.Summary.CreatedAt);
            string message;
            if (time.Days > 0)
            {
                if (time.Days == 1)
                    message = $"Sold within {time.Days} day";
                else
                    message = $"Sold within {time.Days} days";
            }              
            else
            {
                if (time.Hours == 1)
                    message = $"Sold within {time.Hours} hour";
                else
                    message = $"Sold within {time.Hours} hours";
            }
            return message;
        }

        public async Task InsertCar(int userId, CarDTO car)
        {
            var user = await UserRepository.GetUserById(userId);
            if (user == null)
                throw new BusinessException("Cannot add car because such user not found");
            if (await CarRepository.CheckIfUserAlreadyHasCarWithSuchVin(userId, car.Vin))
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
            await CarRepository.InsertCar(carEntity);
            //inserting images

            /*var insertedImages = await InsertImages(carEntity.Id, car.Images);

            //setting default image to the first one of a list
            if (insertedImages.Count < 1)
                carEntity.DefaultImage = Settings.DefaultImageUrl;
            else
                carEntity.DefaultImage = car.Images.FirstOrDefault();

            await CarRepository.UpdateCarDefaultImage(carEntity.Id, carEntity.DefaultImage);*/

            //inserts empty tracking 
            await TrackingRepository.InsertTracking(new Tracking { Vin = car.Vin, CarId = carEntity.Id });

            await SummaryService.InsertCarSummary(carEntity.Id, new SummaryDTO
            {
                BoughtPrice = car.BoughtPrice,
                BoughtPriceCurrency = car.BoughtPriceCurrency,
                BaseCurrency = user.Currency,
                CarId = carEntity.Id
            });

            await ShippingRepository.InsertShipping(new Shipping { CarId = carEntity.Id });
        }
        
        public async Task DeleteCar(int userId, int carId)
        {
            var car = await CarRepository.GetCarById(carId);
            // if (car.User != userId)
            //     throw new BusinessException("Car does not exist");

            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            await CarRepository.DeleteCar(car.Id);
            await FileRepository.DeleteFolder("/cars/" + carId);

            await FileRepository.DeleteFolder("/tracking/" + tracking.Id);
        }

        public async Task UpdateCar(int userId, int carId, Car car)
        {
            // if (car.User != userId)
            //     throw new BusinessException("Car does not exist");

            var list = car.Equipment.Select(x => x.Code).ToList().Distinct().Count();
            if (car.Equipment.Count != list)
                throw new BusinessException("Car cannot have multiple equipment with the same code!");

            await CarRepository.UpdateCar(carId, car);
        }

        public async Task<List<string>> InsertImages(int carId, List<string> images)
        {
            if (images != null && images.Count > 0)
            {
                //deletes form cloud
                await FileRepository.DeleteFolder("/cars/" + carId);
                //deletes from db
                await CarRepository.DeleteAllCarImages(carId);

                var imgsList = new List<UploadImageRequest.File>();

                int count = 1;
                images.ForEach(x => imgsList.Add(
                    new UploadImageRequest.File()
                    {
                        FileName = count++ + ".jpeg",
                        Data = new MemoryStream(FileRepository.Base64ToByteArray(x.Split(',')[1]))
                    }
                ));

                //inserts to cloud 
                var insertedUrls = await FileRepository.InsertCarImages(carId, imgsList);
                //inserts to db
                await CarRepository.UploadImageToCar(carId, insertedUrls);
                return insertedUrls;
            }
            else
            {
                //deletes form cloud
                await FileRepository.DeleteFolder("/cars/" + carId);
                //deletes from db
                await CarRepository.DeleteAllCarImages(carId);
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
                    await FileRepository.DeleteImage(path);
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
                        Data = new MemoryStream(FileRepository.Base64ToByteArray(x.Split(',')[1]))
                    }
                ));

                //inserts to cloud 
                var insertedUrls = await FileRepository.InsertCarImages(carId, imgsList);
                if (insertedUrls != null)
                {
                    insertedUrls.ForEach(x => urls.Add(x));
                }
            }
            await CarRepository.UploadImageToCar(carId, urls);
        }
    }
}
