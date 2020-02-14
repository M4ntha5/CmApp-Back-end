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

namespace Tests
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
            var list1 = new List<Parameter> { new Parameter { Name = "F30", Type = "series" } };
            var list2 = new List<Equipment> { new Equipment { Name = "S4NE", Type = "Blow-by heater" } };

            var cars = new List<CarEntity>
            {
                new CarEntity() { Parameters = list1, Equipment = list2, Id = "1" },
                new CarEntity() { Parameters = list1, Equipment = list2, Id = "1" }
            };

            carMock.GetAllCars().Returns(cars);

            var response = carMock.GetAllCars().Result;

            carMock.Received().GetAllCars();
            Assert.AreEqual(cars.Count, response.Count);
            Assert.AreEqual(cars[0].Parameters[0].Name, response[0].Parameters[0].Name);
            
        }

        [Test]
        public async Task TestInsertScrapedData()
        {
            var eq = new Dictionary<string, string> { { "eq_key1", "eq_val1" } };
            var par = new Dictionary<string, string> { { "par_key1", "par_val1" } };
            var car = new CarEntity
            {
                Equipment = new List<Equipment> { new Equipment() },
                Parameters = new List<Parameter> { new Parameter() },
                Id = "1"
            };

            scraperMock.GetVehicleEquipment(Arg.Any<string>()).Returns(eq);
            scraperMock.GetVehicleInfo(Arg.Any<string>()).Returns(par);
            carMock.InsertCar(Arg.Any<CarEntity>()).Returns(car);

            var carService = new CarService
            {
                CarRepository = carMock,
                WebScraper = scraperMock
            };

            var carDetails = await carService.InsertCarDetailsFromScraper("123");

            Assert.AreEqual(eq.Count, carDetails.Equipment.Count);
            Assert.AreEqual(par.Count, carDetails.Parameters.Count);
            await carMock.Received().InsertCar(Arg.Any<CarEntity>());
        }
    }
}
