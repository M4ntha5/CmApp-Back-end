using CmApp;
using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsTests
{
    class CarServiceTests
    {
        ICarRepository carMock;
        IScraperService scraperMock;
        IFileRepository fileMock;
        ITrackingRepository trackingMock;

        [SetUp]
        public void Setup()
        {
            trackingMock = Substitute.For<ITrackingRepository>();
            fileMock = Substitute.For<IFileRepository>();
            carMock = Substitute.For<ICarRepository>();
            scraperMock = Substitute.For<IScraperService>();
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
        public async Task TestInsertBMWScrapedData()
        {
            var eq = new Dictionary<string, string> { { "eq_key1", "eq_val1" } };
            var par = new Dictionary<string, string> { { "Type", "328" } };
            var car = new CarEntity
            {
                Vin = "123",
                Images = new List<object>(),
                Equipment = new List<Equipment> { new Equipment() },
                Id = "1",
                Make = "BMW"
            };
            var carDet = new CarDisplay
            {
                Vin = "1232",
            };

            carMock.GetAllUserCars(Arg.Any<string>()).Returns(new List<CarDisplay> { carDet });
            scraperMock.GetVehicleEquipment(Arg.Any<string>(), Arg.Any<string>()).Returns(eq);
            scraperMock.GetVehicleInfo(Arg.Any<string>(), Arg.Any<string>()).Returns(par);
            carMock.InsertCar(Arg.Any<CarEntity>()).Returns(car);

            var carService = new CarService
            {
                CarRepository = carMock,
                WebScraper = scraperMock,
                FileRepository = fileMock,
                TrackingRepository = trackingMock
            };

            var carDetails = await carService.InsertCar(car);

            Assert.AreEqual(eq.Count, carDetails.Equipment.Count);
            await carMock.Received().InsertCar(Arg.Any<CarEntity>());
        }
    }
}
