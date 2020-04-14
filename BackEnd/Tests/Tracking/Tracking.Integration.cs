using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using CmApp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tracking.Integration
{
    class Tracking
    {
        TrackingService trackingService;
        TrackingRepository trackingRepo;
        string carId;
        [SetUp]
        public void Setup()
        {
            trackingRepo = new TrackingRepository();
            trackingService = new TrackingService()
            {
                TrackingRepository = trackingRepo,
                ScraperService = new ScraperService(),
                CarRepository = new CarRepository(),
                FileRepository = new FileRepository()
            };
            carId = "5e94b2ee6189921bb45d99a6";
        }

        [Test]
        public async Task TestGetTracking()
        {
            var tracking = await trackingService.GetTracking(carId);
            Assert.AreEqual(carId, tracking.Car);
        }
        [Test]
        public async Task TestInsertTracking()
        {
            var entity = new TrackingEntity()
            {
                AuctionImages = new List<object>(),
                BookingNumber = "123",
                ContainerNumber = "321",
                Url = "myuri",
            };

            var tracking = await trackingService.InsertTracking(carId, entity);
            Assert.AreEqual(carId, tracking.Car);
        }

        [Test]
        public async Task TestUpdateTracking()
        {
            var entity = new TrackingEntity()
            {
                AuctionImages = new List<object>(),
                BookingNumber = "naujas",
                ContainerNumber = "naujas",
                Url = "naujas",
            };
            await trackingService.UpdateTracking(carId, entity);
        }

        [Test]
        public async Task TestDeleteTracking()
        {
            await trackingService.DeleteTracking(carId);
        }
        [Test]
        public async Task TestDeleteTrackingImages()
        {
            await trackingRepo.DeleteTrackingImages("5e94bef563a2911854ac16f1");
        }

        [Test]
        public async Task TestUploadImageToTracking()
        {
            await trackingRepo.UploadImageToTracking(
                "5e94bef563a2911854ac16f1", new byte[0], "test.png");
        }
        [Test]
        public async Task TestUpdateImageShowStatus()
        {
            await trackingRepo.UpdateImageShowStatus(
                "5e94bef563a2911854ac16f1", false);
        }
        [Test]
        public void TestInsertEmpty()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async ()=>
                await trackingRepo.InsertTracking(null));
        }
    }
}
