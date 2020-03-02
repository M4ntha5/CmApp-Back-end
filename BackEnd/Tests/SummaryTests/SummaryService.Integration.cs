using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Integration
{
    class SummaryServiceTests
    {
        SummaryRepository summaryRepo;
        SummaryService serviceRepo;
        string carId;
        string summaryId;

        [SetUp]
        public void Setup()
        {
            summaryRepo = new SummaryRepository();
            serviceRepo = new SummaryService
            {
                SummaryRepository = summaryRepo
            };
            carId = "5e563002ac98df000158536f";
            summaryId = "5e58fa342578380001fdd341";
        }

        [Test]
        public async Task GetSummaryByCarId()
        {
            var summary = await summaryRepo.GetSummaryByCarId(carId);     

            Assert.AreEqual(carId, summary.Car);
        }

        [Test]
        public async Task InsertSummary()
        {
            var entity = new SummaryEntity { BoughtPrice = 25000 };

            var summary = await serviceRepo.InsertCarSummary(carId, entity);

            Assert.AreEqual(entity.BoughtPrice, summary.BoughtPrice);
            Assert.AreEqual(carId, summary.Car);
        }

        [Test]
        public async Task UpdateSummary()
        {
            var entity = new SummaryEntity
            {
                BoughtPrice = 5500,
                Sold = true,
                SoldDate = DateTime.Now,
                SoldPrice = 8000,
            };

            await serviceRepo.UpdateSummary(carId, summaryId, entity);
        }

        [Test]
        public async Task DeleteSummary()
        {
            await serviceRepo.DeleteSummary(carId, summaryId);
        }
    }
}
