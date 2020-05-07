using CmApp;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using CmApp.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperTests
{
    public class ScraperTests
    {
        readonly string Vin = "WBA7E2C37HG740629";
        ScraperService scraperService;
        CarRepository carRepo;
        [SetUp]
        public void Setup()
        {
            scraperService = new ScraperService();
            carRepo = new CarRepository();
        }

        [Test]
        public void TestGetVehicleInfo()
        {
            var vin = "WBS1J5C56JVD36905";
            var equipment = scraperService.GetVehicleInfo(vin, "BMW");
            Assert.IsNotEmpty(equipment);

            vin = "WDDLJ7EB1CA031646";
            equipment = scraperService.GetVehicleInfo(vin, "Mercedes-benz");
            Assert.IsNotEmpty(equipment);

            Assert.Throws<BusinessException>(() =>
                scraperService.GetVehicleInfo(null, "Mercedes-benz"));

        }

        [Test]
        public void TestGetVehicleEquipment()
        {
            var equipment = scraperService.GetVehicleEquipment(Vin, "BMW");
            Assert.IsNotEmpty(equipment);

            Assert.Throws<AggregateException>(() =>
                scraperService.GetVehicleEquipment(null, "BMW"));

            var vin = "WDDLJ7EB1CA031646";
            equipment = scraperService.GetVehicleEquipment(vin, "Mercedes-benz");
            Assert.IsNotEmpty(equipment);
            Assert.Throws<AggregateException>(() =>
                scraperService.GetVehicleEquipment(null, "Mercedes-benz"));

        }

        [Test]
        public async Task TestTrackingScraper()
        {
            //var vin = "WBA7E2C37HG740629";
            //vin = "wba3n9c56ek245582";
            //vin = "WBS1J5C56JVD36905";
            //vin = "WBA3A9G51FNT09002";

            var trackingId = "5ea961711d20e577d470a50e";

            var car = await carRepo.GetCarById("5ea9616f1d20e577d470a50d");
            await scraperService.TrackingScraper(car, trackingId);

        }

        [Test]
        public async Task DownloadAllTrackingImages()
        {
            var tracking = new TrackingEntity()
            {
                Id = "5ea728d644d20049748fed0a",
                AuctionImages = new List<object>()
            };

            var urls = new List<string> { Settings.DefaultImageUrl };

            await scraperService.DownloadAllTrackingImages(tracking, urls);

            await scraperService.DownloadAllTrackingImages(tracking, new List<string>());

            tracking = new TrackingEntity()
            {
                Id = "5ea728d644d20049748fed0a",
                AuctionImages = new List<object> { new { } }
            };

            await scraperService.DownloadAllTrackingImages(tracking, new List<string>());

        }

    }
}
