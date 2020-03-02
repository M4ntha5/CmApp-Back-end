using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Shipping.Integration
{
    class Shipping
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task TestGetShipping()
        {
            var shippingService = new ShippingService()
            {
                ShippingRepository = new ShippingRepository()
            };

            var carId = "5e563002ac98df000158536f";

            var shipping = await shippingService.GetShipping(carId);

            Assert.AreEqual(carId, shipping.Car);
        }

        [Test]
        public async Task TestInsertShipping()
        {
            var shippingService = new ShippingService()
            {
                ShippingRepository = new ShippingRepository()
            };

            var entity = new ShippingEntity
            {
                AuctionFee = 1,
                Customs = 2,
                TransferFee = 3,
                TransportationFee = 4
            };

            var carId = "5e563002ac98df000158536f";

            var shipping = await shippingService.InsertShipping(carId, entity);

            Assert.AreEqual(entity.Car, shipping.Car);
        }

        [Test]
        public async Task TestDeleteShipping()
        {
            var shippingService = new ShippingService()
            {
                ShippingRepository = new ShippingRepository()
            };

            var carId = "5e563002ac98df000158536f";

            await shippingService.DeleteShipping(carId);
        }

        [Test]
        public async Task TestUpdateShipping()
        {
            var shippingService = new ShippingService()
            {
                ShippingRepository = new ShippingRepository()
            };

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
