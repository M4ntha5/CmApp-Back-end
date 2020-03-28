using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class CarService : ICarService
    {
        public ICarRepository CarRepository { get; set; }
        public IWebScraper WebScraper { get; set; }
        public ISummaryRepository SummaryRepository { get; set; }
        public IFileRepository FileRepository { get; set; }

        private async Task<CarEntity> InsertCarDetailsFromScraperBMW(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new BusinessException("Vin number cannot be null or empty!");
            if (car.Make != "BMW")
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
                if (param.Key == "Prod. Date")
                    carEntity.ManufactureDate = Convert.ToDateTime(param.Value);
                else if (param.Key == "Type")
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

            //matching equipment to entity
            var eqResults = WebScraper.GetVehicleEquipment(car.Vin, car.Make);
            var equipment = new List<Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Equipment() { Code = eq.Key, Name = eq.Value });

            carEntity.Equipment = equipment;
            carEntity.Vin = car.Vin.ToUpper();

            //inserting vehicle data
            var insertedCar = await CarRepository.InsertCar(carEntity);

            //image upload here
            if (car.Base64images != null && car.Base64images.Count > 0)
            {
                int count = 1;
                foreach (var image in car.Base64images)
                {
                    //spliting base64 front and getting image format and base64 string 
                    var split = image.Split(';');
                    var imageType = split[0].Split('/')[1];
                    var base64 = split[1].Split(',')[1];
                    var imgName = insertedCar.Id + "_image" + count + "." + imageType;

                    var bytes = FileRepository.Base64ToByteArray(base64);
                    await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
                    count++;
                }
            }
            else
            {
                var defaultImg = await FileRepository.GetFile(Settings.DefaultImage);
                using MemoryStream ms = new MemoryStream();
                defaultImg.CopyTo(ms);
                var bytes = ms.ToArray();
                var imgName = "Default-image.jpg";
                await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
            }
            return insertedCar;

        }

        private async Task<CarEntity> InsertCarDetailsFromScraperMB(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new BusinessException("Vin number cannot be null or empty!");
            if (car.Make != "Mercedes-benz")
                throw new BusinessException("Bad vin for this make!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin, car.Make);

            CarEntity carEntity = new CarEntity
            {
                //default for this scraper
                Make = car.Make,
                User = car.User
            };

            foreach (var param in parResults)
            {
                if (param.Key == "Production Date")
                    carEntity.ManufactureDate = Convert.ToDateTime(param.Value);
                else if (param.Key == "Model")
                    carEntity.Model = param.Value;
                else if (param.Key == "Colour")
                    carEntity.Color = param.Value;
                else if (param.Key == "Upholstery")
                    carEntity.Interior = param.Value;
            }

            //matching equipment to entity
            var eqResults = WebScraper.GetVehicleEquipment(car.Vin, car.Make);
            var equipment = new List<Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Equipment() { Code = eq.Key, Name = eq.Value });

            carEntity.Equipment = equipment;
            carEntity.Vin = car.Vin.ToUpper();

            //inserting vehicle data
            var insertedCar = await CarRepository.InsertCar(carEntity);

            //image upload here
            if (car.Base64images != null && car.Base64images.Count > 0)
            {
                int count = 1;
                foreach (var image in car.Base64images)
                {
                    //spliting base64 front and getting image format and base64 string 
                    var split = image.Split(';');
                    var imageType = split[0].Split('/')[1];
                    var base64 = split[1].Split(',')[1];
                    var imgName = insertedCar.Id + "_image" + count + "." + imageType;

                    var bytes = FileRepository.Base64ToByteArray(base64);
                    await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
                    count++;
                }
            }
            else
            {
                var defaultImg = await FileRepository.GetFile(Settings.DefaultImage);
                using MemoryStream ms = new MemoryStream();
                defaultImg.CopyTo(ms);
                var bytes = ms.ToArray();
                var imgName = "Default-image.jpg";
                await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
            }

            return insertedCar;

        }
        private async Task<CarEntity> InsertOtherCar(CarEntity car)
        {
            if (car == null)
                throw new BusinessException("Can not insert car, because car is empty!");

            car.Vin = car.Vin.ToUpper();
            //inserting vehicle data
            var insertedCar = await CarRepository.InsertCar(car);

            //image upload here
            if (car.Base64images != null && car.Base64images.Count > 0)
            {
                int count = 1;
                foreach (var image in car.Base64images)
                {
                    //spliting base64 begining and getting image format and base64 string 
                    var split = image.Split(';');
                    var imageType = split[0].Split('/')[1];
                    var base64 = split[1].Split(',')[1];
                    var imgName = insertedCar.Id + "_image" + count + "." + imageType;

                    var bytes = FileRepository.Base64ToByteArray(base64);
                    await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
                    count++;
                }
            }
            else
            {
                var defaultImg = await FileRepository.GetFile(Settings.DefaultImage);
                using MemoryStream ms = new MemoryStream();
                defaultImg.CopyTo(ms);
                var bytes = ms.ToArray();
                var imgName = "Default-image.jpg";
                await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
            }
            return insertedCar;
        }

        public async Task<CarEntity> InsertCar(CarEntity car)
        {
            if (car.Make == null || car.Make == "")
                throw new BusinessException("Make not defined");
            if (car.Make == "BMW")
                return await InsertCarDetailsFromScraperBMW(car);
            else if (car.Make == "Mercedes-benz")
                return await InsertCarDetailsFromScraperMB(car);
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
        public async Task<List<CarDisplay>> GetAllUserCars(string userid)
        {
            var cars = await CarRepository.GetAllUserCars(userid);
            if (cars.Count == 0)
                //throw new HttpResponseException() { Value = "You do not have any cars yet!" };
                throw new BusinessException("You do not have any cars yet!");

            foreach (var car in cars)
            {
                if (car.Images.Count != 0)
                {
                    var fileInfo = FileRepository.GetFileId(car.Images[0]);
                    car.Images = null;
                    var fileId = fileInfo.Item1;

                    var url = await FileRepository.GetFileUrl(fileId);

                    car.MainImgUrl = url;
                }
            }
            return cars;
            
        }

        public async Task UpdateCar(string userId, string carId, CarEntity car)
        {
            if (car.User != userId)
                throw new BusinessException("Car does not exist");
            await CarRepository.UpdateCar(carId, car);
        }
        public async Task<List<CarEntity>> GetAllCars()
        {
            var cars = await CarRepository.GetAllCars();
            if (cars.Count == 0)
                //throw new HttpResponseException() {Value = "You do not have any cars yet!" };
                throw new BusinessException("You do not have any cars yet!");

            foreach (var car in cars)
            {
                if(car.Images.Count != 0)
                {
                    var fileInfo = FileRepository.GetFileId(car.Images[0]);
                    car.Images = null;
                    var fileId = fileInfo.Item1;

                    var url = await FileRepository.GetFileUrl(fileId);

                    car.MainImgUrl = url;
                }      
            }
            return cars;
        }
        public async Task<CarEntity> GetCarById(string id)
        {
            var car = await CarRepository.GetCarById(id);
            if (car == null)
                throw new BusinessException("Car with provided id does not exists!");

            car.ManufactureDate = car.ManufactureDate.Date;

            if (car.Images.Count != 0)
            {
                //fetching only first image
                var fileInfo = FileRepository.GetFileId(car.Images[0]);

                var fileId = fileInfo.Item1;
                var fileType = fileInfo.Item2;

                var stream = await FileRepository.GetFile(fileId);

                var mem = new MemoryStream();
                stream.CopyTo(mem);

                var bytes = FileRepository.StreamToByteArray(mem);
                string base64 = FileRepository.ByteArrayToBase64String(bytes);

                base64 = "data:" + fileType + ";base64," + base64;

                car.Base64images.Add(base64);
            }
            return car;
        }

    }
}
