using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using CmApp.Domains;
using CmApp.Services;

namespace Tests
{
    public class ScraperTests
    {
        string Vin = string.Empty;
        [SetUp]
        public void Setup()
        {
            Vin = "wba3b1g58ens79736";
        }

        [Test]
        public void TestGetVehicleInfo()
        {
            var scraper = new WebScraper();
            var equipment = scraper.GetVehicleInfo(Vin);

            Assert.AreNotEqual(null, equipment);
        }

        [Test]
        public void TestGetVehicleEquipment()
        {
            var scraper = new WebScraper();
            var equipment = scraper.GetVehicleEquipment(Vin);

            Assert.AreNotEqual(null, equipment);
        }
    }
}
