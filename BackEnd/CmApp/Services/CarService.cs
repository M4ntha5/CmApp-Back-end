using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Repositories;
using Newtonsoft.Json.Linq;
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
                throw new ArgumentNullException("Vin number cannot be null or empty!");
            if (car.Make != "BMW")
                throw new ArgumentNullException("Bad vin for this make!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin, car.Make);

            CarEntity carEntity = new CarEntity
            {
                Make = car.Make                //default for this scraper
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
            if (car.Drive == "HECK")
                car.Drive = "Rear wheel drive";
            else if (car.Drive == "ALLR")
                car.Drive = "All wheel drive";

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
            if(car.Base64images != null && car.Base64images.Count > 0)
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

            //create summary
            var summaryEntity = new SummaryEntity 
            { 
                BoughtPrice = car.BoughtPrice, 
                Car = insertedCar.Id 
            };
            _ = await SummaryRepository.InsertSummary(summaryEntity);

            return insertedCar;

        }

        private async Task<CarEntity> InsertCarDetailsFromScraperMB(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new ArgumentNullException("Vin number cannot be null or empty!");
            if (car.Make != "Mercedes-benz")
                throw new ArgumentNullException("Bad vin for this make!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin, car.Make);

            CarEntity carEntity = new CarEntity
            {
                Make = car.Make             //default for this scraper
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

            //create summary
            var summaryEntity = new SummaryEntity
            {
                BoughtPrice = car.BoughtPrice,
                Car = insertedCar.Id
            };
            _ = await SummaryRepository.InsertSummary(summaryEntity);

            return insertedCar;

        }
        private async Task<CarEntity> InsertOtherCar(CarEntity car)
        {
            if (car == null)
                throw new ArgumentNullException("Can not insert car, because car is empty!");

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

            //create summary
            var summaryEntity = new SummaryEntity
            {
                BoughtPrice = car.BoughtPrice,
                Car = insertedCar.Id
            };
            _ = await SummaryRepository.InsertSummary(summaryEntity);

            return insertedCar;
        }

        public async Task<CarEntity> InsertCar(CarEntity car)
        {
            if (car.Make == null || car.Make == "")
                throw new Exception("Make not defined");
            if (car.Make == "BMW")
                return await InsertCarDetailsFromScraperBMW(car);
            else if (car.Make == "Mercedes-benz")
                return await InsertCarDetailsFromScraperMB(car);
            else
                return await InsertOtherCar(car);
        }

        public async Task DeleteCar(string id)
        {
            var car = CarRepository.GetCarById(id).Result;

            await CarRepository.DeleteCar(car);
        }

        public async Task UpdateCar(string id, CarEntity car)
        {
            await CarRepository.UpdateCar(id, car);
        }
        public async Task<List<CarEntity>> GetAllCars()
        {
            var cars = await CarRepository.GetAllCars();
                     
            foreach (var car in cars)
            { 
               /* var summary = await SummaryRepository.GetSummaryByCarId(car.Id);
                car.Summary = summary;*/
         
                var fileInfo = FileRepository.GetFileId(car.Images[0]);
                car.Images = null;
                var fileId = fileInfo.Item1;
                var fileType = fileInfo.Item2;

                var url = await FileRepository.GetFileUrl(fileId);

                car.MainImgUrl = url;

              /*  var stream = await FileRepository.GetFile(fileId);

                var mem = new MemoryStream();
                stream.CopyTo(mem);

                var bytes = FileRepository.StreamToByteArray(mem);
                string base64 = FileRepository.ByteArrayToBase64String(bytes);

                base64 = "data:" + fileType + ";base64," + base64;

                car.Base64images.Add(base64);*/
            }
            return cars;
        }
        public async Task<CarEntity> GetCarById(string id)
        {
            var car = await CarRepository.GetCarById(id);
            car.ManufactureDate = car.ManufactureDate.Date;
            foreach (var image in car.Images)
            {
                var fileInfo = FileRepository.GetFileId(image);

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
