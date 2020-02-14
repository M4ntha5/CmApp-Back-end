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

        public async Task<CarEntity> InsertCarDetailsFromScraper(string vin)
        {
            if (vin == null || vin == "")
                throw new ArgumentNullException("Vin number cannot be null or empty!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(vin);
            var parameters = new List<Parameter>();

            foreach(var param in parResults)
                parameters.Add(new Parameter() { Type = param.Key, Name = param.Value });

            //matching equipment to entity
            var eqResults = WebScraper.GetVehicleEquipment(vin);
            var equipment = new List<Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Equipment() { Type = eq.Key, Name = eq.Value });

            var car = new CarEntity() { Equipment = equipment, Parameters = parameters };

            var response = await CarRepository.InsertCar(car);

            return response;

        }

    }
}
