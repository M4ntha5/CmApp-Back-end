using CmApp;
using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepairsTests
{
    class RepairServiceTestsIntegration
    {
        IRepairRepository repairRepo;
        IRepairService repairService;
        string carId;
        string repairId;
        [SetUp]
        public void Setup()
        {
            repairRepo = new RepairRepository();
            repairService = new RepairService()
            {
                RepairRepository = repairRepo,
                SummaryRepository = new SummaryRepository()
            };

            carId = "5ea728c744d20049748fed09";
            repairId = "5ea99138db38aa85d006c808";
        }

        [Test]
        public async Task TestGetAllCarRepairs()
        {
            var repairs = await repairService.GetAllSelectedCarRepairs(carId);
            Assert.AreNotEqual(null, repairs);
        }

        [Test]
        public async Task TestGetSelectedCarRepair()
        {
            var repairs = await repairService.GetSelectedCarRepairById(carId, repairId);
            Assert.AreEqual(repairId, repairs.Id);
            Assert.AreEqual(carId, repairs.Car);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await repairService.GetSelectedCarRepairById(carId, "5ea991381438aa85d006c808"));
        }


        [Test]
        public async Task TestUpdateSelectedCarRepair()
        {
            var repair = new RepairEntity() { Name = "kapotas", Price = 200, Car = carId };
            await repairRepo.UpdateRepair(repairId, repair);
        }

        [Test]
        public async Task TestDeleteSelectedCarRepair()
        {
            await repairRepo.DeleteMultipleRepairs(carId);
        }

        [Test]
        public async Task TestInsertCarRepair()
        {
            var repair = new RepairEntity { Name = "k.p p.sparnas", Price = 250, Car = carId };

            await repairService.InsertCarRepairs(carId, new List<RepairEntity> { repair });

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await repairRepo.InsertRepair(null));
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await repairRepo.InsertMultipleRepairs(null));

        }

    }
}
