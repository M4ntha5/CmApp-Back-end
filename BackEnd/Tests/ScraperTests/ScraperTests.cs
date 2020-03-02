using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using CmApp.Services;
using System.Threading.Tasks;
using CmApp.Repositories;

namespace ScraperTests
{
    public class ScraperTests
    {
        string Vin = string.Empty;
        WebScraper scraperService;

        [SetUp]
        public void Setup()
        {
            Vin = "wba3b1g58ens79736";
            scraperService = new WebScraper
            {
                CarRepo = new CarRepository(),
                FileRepository = new FileRepository(),
                TrackingRepo = new TrackingRepository()
            };
        }

        [Test]
        public void TestGetVehicleInfo()
        {
            var equipment = scraperService.GetVehicleInfo(Vin);

            Assert.AreNotEqual(null, equipment);
        }

        [Test]
        public void TestGetVehicleEquipment()
        {
            var equipment = scraperService.GetVehicleEquipment(Vin);

            Assert.AreNotEqual(null, equipment);
        }

        [Test]
        public async Task TestTrackingScraper()
        {

            var vin = "WBA7E2C37HG740629";
            vin = "wba3n9c56ek245582";
            await scraperService.TrackingScraper(vin);

        }
    }
}
