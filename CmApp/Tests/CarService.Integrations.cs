using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    class CarServiceIntegrtations
    {
        CarRepository carRepo;
        CarService carService;
        WebScraper scraperService;
        [SetUp]
        public void Setup()
        {
            carRepo = new CarRepository();
            scraperService = new WebScraper();
            carService = new CarService()
            {
                CarRepository = carRepo,
                WebScraper = scraperService
            };

        }

        [Test]
        public async Task TestGetAllCars()
        {
            var cars = await carRepo.GetAllCars();

            Assert.AreNotEqual(0, cars.Count);           
        }

        [Test]
        public async Task TestInsertCar()
        {
            string vin = "WBAGE11070DJ00378";
            //var file = await carRepo.UploadImage(vin, "img.jpg");

            var car = new CarEntity
            {
                Images = new List<object>() { "", ""},
                Vin = vin
            };
            var response = await carService.InsertCarDetailsFromScraper(car);

            Assert.AreEqual(vin, response.Vin);
        }

        [Test]
        public async Task TestUpdateCar()
        {
            var carId = "5e4c2dfac0ae1700011a2c38";

            var eq = new Equipment() { Name = "test3", Code = "test4" };
            var list2 = new List<Equipment> { eq };

            var newCar = new CarEntity()
            {
                BodyType = "BodyType",
                Color = "Color",
                Displacement = 2,
                Drive = "Drive",
                ManufactureDate = DateTime.Now,
                Engine = "Engine",
                Interior = "Interior",
                Make = "Make",
                Model = "Model",
                Power = "Power",
                Series = "Series",
                Steering = "Steering",
                Transmission = "Transmission",
                Equipment = list2,
                Images = new List<object>(),
                Vin = "123"

            };
            await carRepo.UpdateCar(carId, newCar);
        }

        [Test]
        public async Task TestDeleteCar()
        {
            var oldCar = await carRepo.GetCarById("5e4c2dfac0ae1700011a2c38");

            await carRepo.DeleteCar(oldCar);
        }

        [Test]
        public async Task TestFileUpload()
        {
            string recordId = "5e4c2d3bc0ae17000119da0b";

            var result = await carRepo.UploadImage(recordId, "img.jpg");
            Assert.AreNotEqual(null, result);
        }

     /*   [Test]
        public async Task TestAddImageToCar()
        {
            var img = new Image { Name = "lajsfhj" };

            await carRepo.AddImageToCar("5e4810c976fdd2000162938b", img);
        }*/


        [Test]
        public async Task TestGetFile()
        {
            var repo = new FileRepository();
            var response = await repo.GetFile("3e682f57-fbe5-4229-ab07-2cab908ca693");
            Assert.AreNotEqual(null, response);
        }
    }
}
