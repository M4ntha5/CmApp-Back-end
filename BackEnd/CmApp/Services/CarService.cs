using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class CarService : ICarService
    {
        public ICarRepository CarRepository { get; set; }
        public IWebScraper WebScraper { get; set; }
        public IFileRepository FileRepository { get; set; }

        public async Task<CarEntity> InsertCarDetailsFromScraper(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new ArgumentNullException("Vin number cannot be null or empty!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin);

            CarEntity carEntity = new CarEntity
            {
                Make = "BMW"                //default for this scraper
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

            //matching equipment to entity
            var eqResults = WebScraper.GetVehicleEquipment(car.Vin);
            var equipment = new List<Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Equipment() { Code = eq.Key, Name = eq.Value });

            carEntity.Equipment = equipment;
            carEntity.Vin = car.Vin;

            var response = await CarRepository.InsertCar(carEntity);

            foreach (var image in car.Images)
                await CarRepository.UploadImageToCar(response.Id, "img.jpg");

            return response;

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
            return cars;
        }
        public async Task<CarEntity> GetCarById(string id)
        {
            var car = await CarRepository.GetCarById(id);

            var fileId = GetFileId(car.Images[0]);

            var fileRepo = new FileRepository();

            var stream = await fileRepo.GetFile(fileId);

            var mem = new MemoryStream();
            stream.CopyTo(mem);

            var bytes = FileRepository.StreamToByteArray(mem);
            string base64 = FileRepository.ByteArrayToBase64String(bytes);

            base64 = "data:image/jpeg;base64,"+base64;

            car.Base64images.Add(base64);

            return car;
        }

        public string GetFileId(object file)
        {
            //all file names list
            var names = file;
            // converting one of the file to string
            var source = names.ToString();
            //parsing formated string json
            dynamic data = JObject.Parse(source);
            //accessing json fields
            string fileId = data.id;
            string fileType = data.originalFileName;

            return fileId;
        }

    }
}
