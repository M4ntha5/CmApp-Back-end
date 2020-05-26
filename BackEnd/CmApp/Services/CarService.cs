using CmApp.Contracts;
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class CarService : ICarService
    {
        public ICarRepository CarRepository { get; set; }
        public IScraperService WebScraper { get; set; }
        public IFileRepository FileRepository { get; set; }
        public ITrackingRepository TrackingRepository { get; set; }

        private async Task<CarEntity> InsertCarDetailsFromScraper(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new BusinessException("Vin number cannot be null or empty!");
            if (car.Make != "BMW" && car.Make != "Mercedes-benz")
                throw new BusinessException("Bad vin for this make!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin, car.Make);

            CarEntity carEntity = new CarEntity
            {
                Make = car.Make,                //default for this scraper
                User = car.User
            };

            foreach (var param in parResults)
            {
                if (param.Key == "Prod. Date" || param.Key == "Production Date")
                    carEntity.ManufactureDate = Convert.ToDateTime(param.Value);
                else if (param.Key == "Type" || param.Key == "Model")
                    carEntity.Model = param.Value;
                else if (param.Key == "Series")
                    carEntity.Series = param.Value;
                else if (param.Key == "Body Type")
                    carEntity.BodyType = param.Value;
                else if (param.Key == "Steering")
                    carEntity.Steering = param.Value;
                else if (param.Key == "Engine")
                    carEntity.Engine = param.Value;
                else if (param.Key == "Displacement")
                    carEntity.Displacement = Double.Parse(param.Value);
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

            var eqResults = WebScraper.GetVehicleEquipment(car.Vin, car.Make);
            var equipment = new List<Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Equipment() { Code = eq.Key, Name = eq.Value });

            carEntity.Equipment = equipment;
            carEntity.Vin = car.Vin.ToUpper();

            //inserting vehicle data
            var insertedCar = await CarRepository.InsertCar(carEntity);

            await InsertImages(insertedCar.Id, car.Base64images);

            //inserts empty tracking 
            await TrackingRepository.InsertTracking(new TrackingEntity { Car = insertedCar.Id });
            return insertedCar;

        }

        private async Task<CarEntity> InsertOtherCar(CarEntity car)
        {
            if (car == null)
                throw new BusinessException("Can not insert car, because car is empty!");

            car.Vin = car.Vin.ToUpper();

            //inserting vehicle data
            var insertedCar = await CarRepository.InsertCar(car);

            await InsertImages(insertedCar.Id, car.Base64images);

            //inserts empty tracking 
            await TrackingRepository.InsertTracking(new TrackingEntity { Car = insertedCar.Id });
            return insertedCar;
        }

        public async Task<CarEntity> InsertCar(CarEntity car)
        {
            var userCars = await CarRepository.GetAllUserCars(car.User);
            var userVins = userCars.Select(x => x.Vin).ToList();

            if (car.Make == null || car.Make == "")
                throw new BusinessException("Make not defined");

            if (userVins.Contains(car.Vin))
                throw new BusinessException("There is already a car with this VIN number");

            if ((car.Make == "BMW" || car.Make == "Mercedes-benz") && car.Model == "")
                return await InsertCarDetailsFromScraper(car);
            else
                return await InsertOtherCar(car);
        }

        public async Task DeleteCar(string userId, string carId)
        {
            var car = await CarRepository.GetCarById(carId);
            if (car.User != userId)
                throw new BusinessException("Car does not exist");

            await CarRepository.DeleteCar(car.Id);
        }

        public async Task UpdateCar(string userId, string carId, CarEntity car)
        {
            if (car.User != userId)
                throw new BusinessException("Car does not exist");

            var list = car.Equipment.Select(x => x.Code).ToList().Distinct().Count();
            if (car.Equipment.Count != list)
                throw new BusinessException("Car cannot have multiple equipment with the same code!");

            await CarRepository.UpdateCar(carId, car);
        }

        public async Task InsertImages(string carId, List<string> images)
        {
            if (images != null && images.Count > 0)
            {
                await CarRepository.DeleteAllCarImages(carId);
                int count = 1;
                foreach (var image in images)
                {
                    //spliting base64 begining and getting image format and base64 string 
                    var split = image.Split(';');
                    var imageType = split[0].Split('/')[1];
                    var base64 = split[1].Split(',')[1];
                    var imgName = carId + "_image" + count + "." + imageType;

                    var bytes = FileRepository.Base64ToByteArray(base64);
                    var res = await CarRepository.UploadImageToCar(carId, bytes, imgName);
                    count++;
                }
            }
            else
                await CarRepository.DeleteAllCarImages(carId);
            
        }

    }
}
