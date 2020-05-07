using CmApp;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using CmApp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CarsTests
{
    class CarServiceIntegrtations
    {
        CarRepository carRepo;
        CarService carService;
        ScraperService scraperService;
        FileRepository fileRepo;
        string carId, userId;

        [SetUp]
        public void Setup()
        {
            carRepo = new CarRepository();
            scraperService = new ScraperService();
            fileRepo = new FileRepository();

            carService = new CarService()
            {
                CarRepository = carRepo,
                WebScraper = scraperService,
                FileRepository = fileRepo,
                SummaryRepository = new SummaryRepository(),
                TrackingRepository = new TrackingRepository()
            };
            carId = "5ea728c744d20049748fed09";
            userId = "5e92d6b981569e0004f1dbbf";
        }

        [Test]
        public async Task TestGetAllCars()
        {
            var cars = await carRepo.GetAllCars();
            Assert.AreNotEqual(0, cars.Count);
        }

        [Test]
        public async Task InsertBMW()
        {
            string vin = "WBA7E2C37HG740629";

            var car = new CarEntity
            {
                Vin = vin,
                Make = "BMW",
                User = userId,
                Base64images = new List<string> { "data:image/png;base64,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" }

            };
            car.Make = null;
            Assert.ThrowsAsync<BusinessException>(async () =>
                await carService.InsertCar(car));
            car.Make = "BMW";
            var response = await carService.InsertCar(car);
            Assert.AreEqual(vin, response.Vin);

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await carRepo.InsertCar(null));

            Assert.ThrowsAsync<BusinessException>(async () =>
                await carService.InsertCar(car));


        }
        [Test]
        public async Task InsertMB()
        {
            string vin = "WDDLJ7EB1CA031646";

            var car = new CarEntity
            {
                Vin = vin,
                Make = "Mercedes-benz",
                User = userId,
                Base64images = new List<string> { "data:image/png;base64,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" }

            };
            var response = await carService.InsertCar(car);
            Assert.AreEqual(vin, response.Vin);

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await carRepo.InsertCar(null));
        }

        [Test]
        public async Task InsertOther()
        {
            string vin = "WDDLJ7EB1CA031111";

            var car = new CarEntity
            {
                Vin = vin,
                Make = "Alfa Romeo",
                Model = "159",
                User = userId,
                Base64images = new List<string> { "data:image/png;base64,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" }

            };
            var response = await carService.InsertCar(car);
            Assert.AreEqual(vin, response.Vin);
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await carRepo.InsertCar(null));
        }

        [Test]
        public async Task TestUpdateCar()
        {
            var newCar = new CarEntity()
            {
                BodyType = "BodyType",
                User = userId,
                Make = "Mercedes-benz",
                Model = "Mercedes-AMG CLS 63 Coupe",
                Vin = "WDDLJ7EB1CA031625",
                Base64images = new List<string> { "data:image/png;base64,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" }
            };
            newCar.User = "";
            Assert.ThrowsAsync<BusinessException>(async () =>
                await carService.UpdateCar(userId, carId, newCar));

            newCar.User = userId;
            newCar.Equipment = new List<Equipment>
                {
                    new Equipment{Code ="123", Name=""},
                    new Equipment{Code = "123", Name=""}
                };
            Assert.ThrowsAsync<BusinessException>(async () =>
                await carService.UpdateCar(userId, carId, newCar));

            newCar.Equipment = new List<Equipment>();
            await carService.UpdateCar(userId, carId, newCar);
        }

        [Test]
        public async Task TestDeleteCar()
        {
            var oldCar = await carRepo.GetCarById(carId);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await carService.DeleteCar("", oldCar.Id));

            await carService.DeleteCar(userId, oldCar.Id);
        }

        [Test]
        public async Task TestGetCarById()
        {
            var car = await carRepo.GetCarById(carId);
            Assert.AreEqual(carId, car.Id);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await carRepo.GetCarById("5ea93b953ebbca201071af71"));
        }

        [Test]
        public async Task TestFileUpload()
        {
            var stream = await fileRepo.GetFile(Settings.DefaultImage);

            var mem = new MemoryStream();
            stream.CopyTo(mem);
            var bytes = fileRepo.StreamToByteArray(mem);
            var result = await carRepo.UploadImageToCar(carId, bytes, "img.png");
            Assert.AreNotEqual(null, result);
        }

        [Test]
        public async Task TestGetFile()
        {
            var response = await fileRepo.GetFile(Settings.DefaultImage);

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
                Assert.IsNotEmpty(base64);
            }
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task DeleteAllCarImages()
        {
            await carRepo.DeleteAllCarImages(carId);
        }
        [Test]
        public async Task GetAllMakes()
        {
            var makes = await carRepo.GetAllMakes();
            Assert.IsNotEmpty(makes);
        }

    }
}
