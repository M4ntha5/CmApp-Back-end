using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarsTests
{
    class CarServiceTests
    {
        ICarRepository carMock;
        IWebScraper scraperMock;

        [SetUp]
        public void Setup()
        {
            carMock = Substitute.For<ICarRepository>();
            scraperMock = Substitute.For<IWebScraper>();
        }

        [Test]
        public void TestGetAllCars()
        {
            var list2 = new List<Equipment> { new Equipment { Name = "S4NE", Code = "Blow-by heater" } };

            var cars = new List<CarEntity>
            {
                new CarEntity() { Model = "330", Equipment = list2, Id = "1" },
                new CarEntity() { Model = "320", Equipment = list2, Id = "2" }
            };

            carMock.GetAllCars().Returns(cars);

            var response = carMock.GetAllCars().Result;

            carMock.Received().GetAllCars();
            Assert.AreEqual(cars.Count, response.Count);
            Assert.AreEqual(cars[0].Model, response[0].Model);
            
        }

        [Test]
        public async Task TestInsertScrapedData()
        {
            var eq = new Dictionary<string, string> { { "eq_key1", "eq_val1" } };
            var par = new Dictionary<string, string> { { "Type", "328" } };
            var car = new CarEntity
            {
                Vin = "123",
                Images = new List<object>(),
                Equipment = new List<Equipment> { new Equipment() },
                Model = "328",
                Id = "1"
            };

            scraperMock.GetVehicleEquipment(Arg.Any<string>(), Arg.Any<string>()).Returns(eq);
            scraperMock.GetVehicleInfo(Arg.Any<string>(), Arg.Any<string>()).Returns(par);
            carMock.InsertCar(Arg.Any<CarEntity>()).Returns(car);

            var carService = new CarService
            {
                CarRepository = carMock,
                WebScraper = scraperMock
            };

            var carDetails = await carService.InsertCarDetailsFromScraperBMW(car);

            Assert.AreEqual(eq.Count, carDetails.Equipment.Count);
            await carMock.Received().InsertCar(Arg.Any<CarEntity>());
        }
    }
}
