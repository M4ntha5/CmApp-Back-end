using CmApp.Entities;
using CmApp.Repositories;
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

        [SetUp]
        public void Setup()
        {
            carRepo = new CarRepository();
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
            var eq = new Equipment { Name = "test1", Type = "test2" };
            var param = new Parameter { Name = "test11", Type = "test22" };          

            var list1 = new List<Parameter> { param };
            var list2 = new List<Equipment> { eq };

            string vin = "WBAGE11070DJ00378";
            var file = await carRepo.UploadImage(vin, "img.jpg");

            var images = new List<Image> { new Image { Name = file.Key } };

            var car = new CarEntity
            {
                Equipment= list2,
                Parameters = list1,
                Images = images,
                Vin = vin
            };
            var response = await carRepo.InsertCar(car);

            Assert.AreEqual(eq.Name, response.Equipment[0].Name);
        }

        [Test]
        public async Task TestUpdateCar()
        {
            var carId = "5e46e9d376fdd200014383cb";

            var param = new Parameter() { Name = "test33", Type = "test44" };
            var eq = new Equipment() { Name = "test3", Type = "test4" };

            var list1 = new List<Parameter> { param };
            var list2 = new List<Equipment> { eq };

            var newCar = new CarEntity()
            {
                Equipment = list2,
                Parameters = list1,
                Vin = "123",
                Images = new List<Image>()

            };
            await carRepo.UpdateCar(carId, newCar);
        }

        [Test]
        public async Task TestDeleteCar()
        {
            var oldCar = await carRepo.GetCarById("5e46e9d376fdd200014383cb");

            await carRepo.DeleteCar(oldCar);
        }

        [Test]
        public async Task TestFileUpload()
        {
            string vin = "WBAGE11070DJ00378";

            var result = await carRepo.UploadImage(vin, "img.jpg");

        }

        [Test]
        public async Task TestAddImageToCar()
        {
            var img = new Image { Name = "lajsfhj" };

            await carRepo.AddImageToCar("5e4810c976fdd2000162938b", img);
        }


        [Test]
        public async Task TestGetFile()
        {
            var repo = new FileRepository();
            var response = await repo.GetFile("5e2c8ac1-1d69-4b2a-a249-48763080a7ee");
        }
    }
}
