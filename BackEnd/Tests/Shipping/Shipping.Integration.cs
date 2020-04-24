using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Shipping.Integration
{
    class Shipping
    {
        ShippingService shippingService;
        ShippingRepository shippingRepo;
        string carId;
        [SetUp]
        public void Setup()
        {
            shippingRepo = new ShippingRepository();
            shippingService = new ShippingService()
            {
                ShippingRepository = shippingRepo,
                SummaryRepository = new SummaryRepository(),
                ExchangeRepository = new ExchangeService()
            };
            carId = "5e94b2ee6189921bb45d99a6";
        }

        [Test]
        public async Task TestGetShipping()
        {
            var shipping = await shippingService.GetShipping(carId);
            Assert.AreEqual(carId, shipping.Car);
        }

        [Test]
        public async Task TestInsertShipping()
        {
            var entity = new ShippingEntity
            {
                AuctionFee = 1,
                AuctionFeeCurrency = "EUR",
                Customs = 2,
                TransferFee = 3,
                TransportationFee = 4,
                TransportationFeeCurrency = "EUR",
                CustomsCurrency = "EUR",
                TransferFeeCurrency = "EUR",
                BaseCurrency = "EUR",
            };

            var shipping = await shippingService.InsertShipping(carId, entity);

            Assert.AreEqual(entity.Car, shipping.Car);
        }

        [Test]
        public async Task TestDeleteShipping()
        {
            await shippingService.DeleteShipping(carId);
        }
        [Test]
        public void TestInsertEmptyShipping()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await shippingRepo.InsertShipping(null));
        }

        [Test]
        public async Task TestUpdateShipping()
        {
            var entity = new ShippingEntity
            {
                AuctionFee = 1,
                AuctionFeeCurrency = "EUR",
                Customs = 2,
                TransferFee = 3,
                TransportationFee = 4,
                TransportationFeeCurrency = "EUR",
                CustomsCurrency = "EUR",
                TransferFeeCurrency = "EUR",
                BaseCurrency = "EUR",
            };

            await shippingService.UpdateShipping(carId, entity);
        }
    }
}
