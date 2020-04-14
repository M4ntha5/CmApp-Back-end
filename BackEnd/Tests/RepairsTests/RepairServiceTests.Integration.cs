using CmApp.Entities;
using CmApp.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepairsTests.Integration
{
    class RepairServiceTestsIntegration
    {
        RepairRepository repairRepo;
        string carId;
        string repairId;
        [SetUp]
        public void Setup()
        {
            repairRepo = new RepairRepository();
            carId = "5e94b2ee6189921bb45d99a6";
            repairId = "5e94b93a99ad482e68e3761a";
        }

        [Test]
        public async Task TestGetAllCarRepairs()
        {
            var repairs = await repairRepo.GetAllRepairsByCarId(carId);
            Assert.AreNotEqual(null, repairs);
        }

        [Test]
        public async Task TestGetSelectedCarRepair()
        {
            var repairs = await repairRepo.GetCarRepairById(carId, repairId);

            Assert.AreEqual(repairId, repairs.Id);
            Assert.AreEqual(carId, repairs.Car);
        }

        [Test]
        public async Task TestDeleteSelectedCarRepair()
        {
            var response = await repairRepo.DeleteRepair(carId, repairId);
            Assert.IsTrue(response.IsAcknowledged);
            Assert.AreEqual(1, response.DeletedCount);
        }

        [Test]
        public async Task TestUpdateSelectedCarRepair()
        {
            var repair = new RepairEntity() { Name = "kapotas", Price = 200, Car = carId };
            await repairRepo.UpdateRepair(repairId, repair);
        }

        [Test]
        public async Task TestInsertCarRepair()
        {
            var repair = new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId };

            var response = await repairRepo.InsertRepair(repair);

            Assert.AreEqual(carId, response.Car);
            Assert.AreEqual(repair.Name, response.Name);
        }
        [Test]
        public async Task TestDeleMultipleCarRepairs()
        {
            await repairRepo.DeleteMultipleRepairs(carId);
        }

        [Test]
        public async Task TestInsertMultipleRepairs()
        {
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
        [Test]
        public void TestInsertEmpty()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await repairRepo.InsertRepair(null));
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await repairRepo.InsertMultipleRepairs(null));
        }
    }
}
