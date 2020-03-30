using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using CmApp.Utils;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Shipping.Integration
{
    class Shipping
    {
        ShippingService shippingService;

        [SetUp]
        public void Setup()
        {
            Settings.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
            Settings.CaptchaApiKey = Environment.GetEnvironmentVariable("CaptchaApiKey");
            shippingService = new ShippingService()
            {
                ShippingRepository = new ShippingRepository(),
                SummaryRepository = new SummaryRepository(),
                ExchangesRepository = new ExchangeRatesRepository()
            };
        }

        [Test]
        public async Task TestGetShipping()
        {
            var carId = "5e563002ac98df000158536f";

            var shipping = await shippingService.GetShipping(carId);

            Assert.AreEqual(carId, shipping.Car);
        }

        [Test]
        public async Task TestInsertShipping()
        {

            var entity = new ShippingEntity
            {
                AuctionFee = 1,
                Customs = 2,
                TransferFee = 3,
                TransportationFee = 4,
            };

            var carId = "5e563002ac98df000158536f";

            var shipping = await shippingService.InsertShipping(carId, entity);

            Assert.AreEqual(entity.Car, shipping.Car);
        }

        [Test]
        public async Task TestDeleteShipping()
        {

            var carId = "5e563002ac98df000158536f";

            await shippingService.DeleteShipping(carId);
        }

        [Test]
        public async Task TestUpdateShipping()
        {
            var entity = new ShippingEntity
            {
                AuctionFee = 11,
                Customs = 22,
                TransferFee = 33,
                TransportationFee = 44
            };

            var carId = "5e563002ac98df000158536f";

            await shippingService.UpdateShipping(carId, entity);
        }
    }
}
