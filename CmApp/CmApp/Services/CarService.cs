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
        public IWebScraper WebScraper { get; set; }

        public async Task<CarEntity> InsertCarDetailsFromScraper(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new ArgumentNullException("Vin number cannot be null or empty!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin);
            var parameters = new List<Parameter>();

            foreach(var param in parResults)
                parameters.Add(new Parameter() { Type = param.Key, Name = param.Value });

            //matching equipment to entity
            var eqResults = WebScraper.GetVehicleEquipment(car.Vin);
            var equipment = new List<Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Equipment() { Type = eq.Key, Name = eq.Value });

            var newCar = new CarEntity 
            { 
                Equipment = equipment, 
                Parameters = parameters,
                Vin = car.Vin,
                Images = car.Images
            };

            var response = await CarRepository.InsertCar(newCar);

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
            return car;
        }

    }
}
