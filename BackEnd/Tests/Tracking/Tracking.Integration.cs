using CmApp;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using CmApp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TrackingTests
{
    class Tracking
    {
        TrackingService trackingService;
        TrackingRepository trackingRepo;
        FileRepository fileRepo;
        string carId;
        [SetUp]
        public void Setup()
        {
            trackingRepo = new TrackingRepository();
            fileRepo = new FileRepository();
            trackingService = new TrackingService()
            {
                TrackingRepository = trackingRepo,
                ScraperService = new ScraperService(),
                CarRepository = new CarRepository(),
                FileRepository = fileRepo
            };
            carId = "5ea728c744d20049748fed09";
        }

        [Test]
        public async Task TestGetTracking()
        {
            var tracking = await trackingRepo.GetTrackingByCar(carId);
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
                Car = carId
            };

            var tracking = await trackingRepo.InsertTracking(entity);
            Assert.AreEqual(carId, tracking.Car);
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await trackingRepo.InsertTracking(null));
        }

        [Test]
        public async Task TestUpdateTracking()
        {
            var entity = new TrackingEntity()
            {
                AuctionImages = new List<object>(),
                BookingNumber = "naujas2",
                ContainerNumber = "naujas2",
                Url = "naujas2",
            };
            await trackingService.UpdateTracking(carId, entity);
        }

        [Test]
        public async Task TestDeleteTracking()
        {
            await trackingRepo.DeleteCarTracking(carId);
        }
        [Test]
        public async Task TestDeleteTrackingImages()
        {
            await trackingRepo.DeleteTrackingImages("5ea961711d20e577d470a50e");
        }

        [Test]
        public async Task TestUploadImageToTracking()
        {
            var stream = await fileRepo.GetFile(Settings.DefaultImage);

            var mem = new MemoryStream();
            stream.CopyTo(mem);
            var bytes = fileRepo.StreamToByteArray(mem);

            await trackingRepo.UploadImageToTracking(
                "5ea71c52862b9f00040c7726", bytes, "test.png");
        }
        [Test]
        public async Task TestUpdateImageShowStatus()
        {
            await trackingService.SaveLastShowImagesStatus(
                "5ea9616f1d20e577d470a50d", false);

            Assert.ThrowsAsync<BusinessException>(async () =>
                await trackingService.SaveLastShowImagesStatus(
                    "5ea9616f1d20e5775570a50d", false));
        }

        [Test]
        public async Task LookForTrackingData()
        {
            await trackingService.LookForTrackingData("5ea9616f1d20e577d470a50d");
            await trackingService.LookForTrackingImages("5ea9616f1d20e577d470a50d");

            Assert.ThrowsAsync<BusinessException>(async () =>
                 await trackingService.LookForTrackingImages("5ea9616f1550e577d470a50d"));

            Assert.ThrowsAsync<BusinessException>(async () =>
                 await trackingService.LookForTrackingData("5ea9616f1d20e5711470a50d"));

        }
        [Test]
        public async Task DownloadTrackingImages()
        {
            await trackingService.DownloadTrackingImages("5ea9616f1d20e577d470a50d",
                new List<string> { Settings.DefaultImageUrl });

            Assert.ThrowsAsync<BusinessException>(async () =>
                 await trackingService.DownloadTrackingImages("5ea9616f1d20e577d550a50d",
                new List<string> { Settings.DefaultImageUrl }));
        }
    }
}
