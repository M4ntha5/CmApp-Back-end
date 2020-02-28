using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.RepairsTests
{
    class RepairServiceTestsIntegration
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestGetAllCarRepairs()
        {
            var carId = "5e4c2e24c0ae1700011a2c3f";

            var repo = new RepairRepository();

            var repairs = await repo.GetAllRepairsByCarId(carId);

            Assert.AreNotEqual(null, repairs);

        }

        [Test]
        public async Task TestGetSelectedCarRepair()
        {
            var carId = "5e4c2e24c0ae1700011a2c3f";

            var repo = new RepairRepository();

            var repairId = "5e4c38a4c0ae1700011d9fb0";

            var repairs = await repo.GetCarRepairById(carId, repairId);

            Assert.AreEqual(repairId, repairs.Id);
            Assert.AreEqual(carId, repairs.Car);
        }

        [Test]
        public async Task TestDeleteSelectedCarRepair()
        {
            var carId = "5e4c2d3bc0ae17000119da0b";

            var repo = new RepairRepository();

            var repairId = "5e4c544dc0ae17000122ce73";

            var response = await repo.DeleteRepair(carId, repairId);
            Assert.IsTrue(response.IsAcknowledged);
            Assert.AreEqual(1, response.DeletedCount);
        }

        [Test]
        public async Task TestUpdateSelectedCarRepair()
        {
            var carId = "5e4c2d3bc0ae17000119da0b";

            var repo = new RepairRepository();

            var repairId = "5e4d91cb0bf52c0001ade71a";

            var repair = new RepairEntity() { Name = "kapotas", Price = 200, Car = carId };

            await repo.UpdateRepair(repairId, repair);
        }

        [Test]
        public async Task TestInsertCarRepair()
        {
            var carId = "5e4c2d3bc0ae17000119da0b";

            var repo = new RepairRepository();

            var repair = new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId };

            var response = await repo.InsertRepair(repair);

            Assert.AreEqual(carId, response.Car);
            Assert.AreEqual(repair.Name, response.Name);
        }
    }
}
