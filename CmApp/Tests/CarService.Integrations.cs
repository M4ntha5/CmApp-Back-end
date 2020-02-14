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
            var eq = new Equipment() { Name = "test1", Type = "test2" };
            var param = new Parameter() { Name = "test11", Type = "test22" };

            var list1 = new List<Parameter> { param };
            var list2 = new List<Equipment> { eq };

            var car = new CarEntity()
            {
                Equipment= list2,
                Parameters = list1
            };
            var response = await carRepo.InsertCar(car);

            Assert.AreEqual(eq.Name, response.Equipment[0].Name);
        }

        [Test]
        public async Task TestUpdateCar()
        {
            var oldCar = await carRepo.GetCarById("5e46e9d376fdd200014383cb");

            var param = new Parameter() { Name = "test33", Type = "test44" };
            var eq = new Equipment() { Name = "test3", Type = "test4" };

            var list1 = new List<Parameter> { param };
            var list2 = new List<Equipment> { eq };

            var newCar = new CarEntity()
            {
                Id = oldCar.Id,
                Equipment = list2,
                Parameters = list1
            };
            await carRepo.UpdateCar(newCar);
        }

        [Test]
        public async Task TestDeleteCar()
        {
            var oldCar = await carRepo.GetCarById("5e46e9d376fdd200014383cb");

            await carRepo.DeleteCar(oldCar);
        }
    }
}
