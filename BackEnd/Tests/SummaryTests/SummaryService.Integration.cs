using CmApp;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using CmApp.Utils;
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

        [SetUp]
        public void Setup()
        {
            summaryRepo = new SummaryRepository();
            serviceRepo = new SummaryService
            {
                SummaryRepository = summaryRepo,
                ExchangeRepository = new ExchangeService()
            };
            carId = "5e94b2ee6189921bb45d99a6";
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
            var entity = new SummaryEntity 
            { 
                BoughtPrice = 25000, 
                BaseCurrency="EUR" ,
                SelectedCurrency = "EUR",
            };
            var summary = await serviceRepo.InsertCarSummary(carId, entity);

            Assert.AreEqual(entity.BoughtPrice, summary.BoughtPrice);
            Assert.AreEqual(carId, summary.Car);
        }
        [Test]
        public void InsertEmptySummary()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                   await summaryRepo.InsertSummary(null));
        }

        [Test]
        public async Task UpdateSummary()
        {
            var entity = new SummaryEntity
            {
                Sold = true,
                SoldDate = DateTime.Now,
                SoldPrice = 8000,
            };
            await serviceRepo.UpdateSoldSummary(carId, entity);
        }

        [Test]
        public async Task DeleteSummary()
        {
            await serviceRepo.DeleteSummary(carId);
        }
    }
}
