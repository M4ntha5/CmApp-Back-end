using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using CmApp.Utils;
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
        ScraperService scraperService;
        FileRepository fileRepo;
        string carId;

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
            carId = "5e94b2ee6189921bb45d99a6";
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
            string vin = "WBA7E2C37HG740629";

            var car = new CarEntity
            {
                Vin = vin,
                Make = "BMW"
            };
            var response = await carService.InsertCar(car);
            Assert.AreEqual(vin, response.Vin);
        }

        [Test]
        public async Task TestUpdateCar()
        {
            var newCar = new CarEntity()
            {
                BodyType = "BodyType",
                User = "",
                Make = "Mercedes-benz",
                Model = "Mercedes-AMG CLS 63 Coupe",
                Vin = "WDDLJ7EB1CA031646"
            };
            await carRepo.UpdateCar(carId, newCar);
        }

        [Test]
        public async Task TestDeleteCar()
        {
            var oldCar = await carRepo.GetCarById(carId);

            await carRepo.DeleteCar(oldCar.Id);
        }

        [Test]
        public async Task TestGetCarById()
        {
            var car = await carService.GetCarById(carId);
            Assert.AreEqual(carId, car.Id);
        }
        [Test]
        public async Task TestGetCarByVin()
        {
            var vin = "WDDLJ7EB1CA031646";
            var car = await carRepo.GetCarByVin(vin);

            Assert.AreEqual(vin, car.Vin);
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
        public void TestInsertEmpty()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await carRepo.InsertCar(null));
        }

        [Test]
        public async Task GetAllUserCars()
        {
            var userId = "5e94b45c9b897c056c2a0a97";
            var cars = await carRepo.GetAllUserCars(userId);
            Assert.IsNotEmpty(cars);
        }
        [Test]
        public async Task DeleteAllCarImages()
        {
            await carRepo.DeleteAllCarImages(carId);
        }
    }
}
