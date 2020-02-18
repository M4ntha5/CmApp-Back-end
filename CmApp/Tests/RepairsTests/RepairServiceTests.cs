using CmApp.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.RepairsTests
{
    class RepairServiceTests
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

            var repairs = await repo.GetRepairById(repairId);

            Assert.AreNotEqual(null, repairs);

        }
    }
}
