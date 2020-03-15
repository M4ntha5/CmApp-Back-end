using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cars.Integration
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
            var response = await carService.InsertCar(car);

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
        public async Task TestGetCarById()
        {
            var service = new CarService() { CarRepository = carRepo, WebScraper = new WebScraper() };

            var carid = "5e4c2d3bc0ae17000119da0b";
            var car = await service.GetCarById(carid);

            Assert.AreEqual(carid, car.Id);
        }
        [Test]
        public async Task TestGetCarByVin()
        {
            var service = new CarService() { CarRepository = carRepo, WebScraper = new WebScraper() };
            var repo = new CarRepository();

            var vin = "WDDLJ7EB1CA031646";
            var car = await repo.GetCarByVin(vin);

            Assert.AreEqual(vin, car.Vin);
        }

        [Test]
        public async Task TestFileUpload()
        {
            string recordId = "5e4c2d3bc0ae17000119da0b";

            var fileRepo = new FileRepository();
            var stream = await fileRepo.GetFile("265b0467-f1fc-4579-8f87-9dae7877c45a");
            var mem = new MemoryStream();
            stream.CopyTo(mem);

            var bytes = fileRepo.StreamToByteArray(mem);



            var result = await carRepo.UploadImageToCar(recordId, bytes, "img.png");
            //Assert.AreNotEqual(null, result);
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
            var response = await repo.GetFile("265b0467-f1fc-4579-8f87-9dae7877c45a");

            var mem = new MemoryStream();

            response.CopyTo(mem);

            if (mem.Length != 0)
            {
                mem.Seek(0, SeekOrigin.Begin);
                int count = 0;
                byte[] byteArray = new byte[mem.Length];
                while (count < mem.Length)
                {
                    byteArray[count++] = Convert.ToByte(mem.ReadByte());
                }
                var base64 = Convert.ToBase64String(byteArray);
            }


            
            

            Assert.AreNotEqual(null, response);
        }
    }
}
