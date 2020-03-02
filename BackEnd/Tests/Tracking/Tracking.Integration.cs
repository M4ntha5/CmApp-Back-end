using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tracking.Integration
{
    class Tracking
    {
        TrackingService trackingService;
        [SetUp]
        public void Setup()
        {
            trackingService = new TrackingService()
            {
                TrackingRepository = new TrackingRepository(),
            };
        }

        [Test]
        public async Task TestGetTracking()
        {
            var carId = "5e5b8bdf257838000157a023";

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
            var carId = "5e5b8bdf257838000157a023";

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
            var carId = "5e5b8bdf257838000157a023";

            await trackingService.UpdateTracking(carId, entity);
        }

        [Test]
        public async Task TestDeleteTracking()
        {
            var carId = "5e562fd1ac98df000158536c";

            await trackingService.DeleteTracking(carId);
        }
    }
}
