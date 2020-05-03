using CmApp;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ShippingTests
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
            carId = "5ea728c744d20049748fed09";
        }

        [Test]
        public async Task TestGetShipping()
        {
            var shipping = await shippingRepo.GetShippingByCar(carId);
            Assert.AreEqual(carId, shipping.Car);
        }

        [Test]
        public async Task TestInsertShipping()
        {
            var entity = new ShippingEntity
            {
                AuctionFee = 1,
                AuctionFeeCurrency = "USD",
                Customs = 2,
                CustomsCurrency = "USD",
                TransferFee = 3,
                TransferFeeCurrency = "USD",
                TransportationFee = 4,
                TransportationFeeCurrency = "USD",
                BaseCurrency = "EUR",
            };
            entity.AuctionFeeCurrency = null;
            Assert.ThrowsAsync<BusinessException>(async () =>
                await shippingService.InsertShipping(carId, entity));

            entity.AuctionFeeCurrency = "USD";
            var shipping = await shippingService.InsertShipping(carId, entity);

            Assert.AreEqual(carId, shipping.Car);
        }

        [Test]
        public async Task TestDeleteShipping()
        {
            await shippingRepo.DeleteCarShipping(carId);
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
