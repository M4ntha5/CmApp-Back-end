using CmApp;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace SummaryTests
{
    class SummaryServiceTests
    {
        SummaryRepository summaryRepo;
        CarService carService;
        string carId, summaryId;

        [SetUp]
        public void Setup()
        {
            summaryRepo = new SummaryRepository();
            carService = new CarService
            {
                SummaryRepository = summaryRepo,
                ExternalAPIService = new ExternalAPIService()
            };
            carId = "5ea728c744d20049748fed09";
            summaryId = "5ea930c8f3c276556c7a4cc0";
        }

        [Test]
        public async Task GetSummaryByCarId()
        {
            var summary = await summaryRepo.GetSummaryByCarId(carId);
            Assert.AreEqual(carId, summary.Car);
        }
        [Test]
        public async Task InsertTotalByCar()
        {
            await summaryRepo.InsertTotalByCar(summaryId, 50);
        }

        [Test]
        public async Task InsertSummary()
        {
            var entity = new SummaryEntity
            {
                BoughtPrice = 25000,
                BaseCurrency = "",
                SelectedCurrency = "EUR",
            };
            Assert.ThrowsAsync<BusinessException>(async () =>
                await carService.InsertCarSummary(carId, entity));

            entity = new SummaryEntity
            {
                BoughtPrice = 25000,
                BaseCurrency = "EUR",
                SelectedCurrency = "USD",
            };
            var summary = await carService.InsertCarSummary(carId, entity);
            Assert.AreEqual(carId, summary.Car);

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
            await carService.UpdateSoldSummary(carId, entity);

            entity.CreatedAt = new DateTime(2020, 04, 28);
            await carService.UpdateSoldSummary(carId, entity);

            entity.CreatedAt = new DateTime(2020, 04, 20);
            await carService.UpdateSoldSummary(carId, entity);

            entity.CreatedAt = new DateTime(2020, 04, 29, 12, 3, 0);
            await carService.UpdateSoldSummary(carId, entity);

            entity.CreatedAt = new DateTime(2020, 04, 29, 15, 35, 0);
            await carService.UpdateSoldSummary(carId, entity);
        }

        [Test]
        public async Task DeleteSummary()
        {
            await summaryRepo.DeleteCarSummary(carId);
        }
    }
}
