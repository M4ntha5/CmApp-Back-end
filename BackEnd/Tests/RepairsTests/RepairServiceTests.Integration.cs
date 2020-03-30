using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepairsTests.Integration
{
    class RepairServiceTestsIntegration
    {
        IRepairRepository repairRepo;

        [SetUp]
        public void Setup()
        {
            Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            Settings.CaptchaApiKey = Environment.GetEnvironmentVariable("CaptchaApiKey");
            repairRepo = new RepairRepository();
        }

        [Test]
        public async Task TestGetAllCarRepairs()
        {
            var carId = "5e4c2e24c0ae1700011a2c3f";

            var repairs = await repairRepo.GetAllRepairsByCarId(carId);

            Assert.AreNotEqual(null, repairs);

        }

        [Test]
        public async Task TestGetSelectedCarRepair()
        {
            var carId = "5e4c2e24c0ae1700011a2c3f";
            var repairId = "5e4c38a4c0ae1700011d9fb0";

            var repairs = await repairRepo.GetCarRepairById(carId, repairId);

            Assert.AreEqual(repairId, repairs.Id);
            Assert.AreEqual(carId, repairs.Car);
        }

        [Test]
        public async Task TestDeleteSelectedCarRepair()
        {
            var carId = "5e4c2d3bc0ae17000119da0b";
            var repairId = "5e4c544dc0ae17000122ce73";

            var response = await repairRepo.DeleteRepair(carId, repairId);
            Assert.IsTrue(response.IsAcknowledged);
            Assert.AreEqual(1, response.DeletedCount);
        }

        [Test]
        public async Task TestUpdateSelectedCarRepair()
        {
            var carId = "5e4c2d3bc0ae17000119da0b";
            var repairId = "5e4d91cb0bf52c0001ade71a";

            var repair = new RepairEntity() { Name = "kapotas", Price = 200, Car = carId };

            await repairRepo.UpdateRepair(repairId, repair);
        }

        [Test]
        public async Task TestInsertCarRepair()
        {
            var carId = "5e4c2d3bc0ae17000119da0b";

            var repair = new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId };

            var response = await repairRepo.InsertRepair(repair);

            Assert.AreEqual(carId, response.Car);
            Assert.AreEqual(repair.Name, response.Name);
        }
        [Test]
        public async Task TestDeleMultipleCarRepairs()
        {
            var carId = "5e82245621f01c255c300cdc";        

            await repairRepo.DeleteMultipleRepairs(carId);
        }


        [Test]
        public async Task TestInsertMultipleRepairs()
        {
            var carId = "5e82245621f01c255c300cdc";

            var repairs = new List<RepairEntity>
            {
                new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId },
                new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId },
                new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId },
                new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId },
                new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId },
                new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId },
                new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId }
            };

            await repairRepo.InsertMultipleRepairs(repairs);
        }
    }
}
