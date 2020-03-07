using CmApp.Contracts;
using CmApp.Entities;
using System;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class ShippingService : IShippingService
    {
        public IShippingRepository ShippingRepository { get; set; }
        public ISummaryRepository SummaryRepository { get; set; }
        public IExchangeRatesRepository ExchangesRepository { get; set; }

        public async Task DeleteShipping(string carId)
        {
            await ShippingRepository.DeleteCarShipping(carId);
        }
        public async Task UpdateShipping(string carId, ShippingEntity shipping)
        {
            shipping.Car = carId;
            var shippingId = ShippingRepository.GetShippingByCar(carId).Result.Id;
            await ShippingRepository.UpdateCarShipping(shippingId, shipping);
        }
        public async Task<ShippingEntity> GetShipping(string carId)
        {
            var shipping = await ShippingRepository.GetShippingByCar(carId);
            return shipping;
        }
        public async Task<ShippingEntity> InsertShipping(string carId, ShippingEntity shipping)
        {
            shipping.Car = carId;
            var newShipping = await ShippingRepository.InsertShipping(shipping);
           
            double totalPrice = newShipping.Customs + newShipping.AuctionFee +
                newShipping.TransferFee + newShipping.TransportationFee;

            var rates = await ExchangesRepository.GetSelectedExchangeRate("USD");

            if (rates.Rates.Count != 1)
                throw new Exception("Bad currency");
                
            var price = totalPrice * double.Parse(rates.Rates["USD"]);

            var summary = await SummaryRepository.GetSummaryByCarId(carId);
            await SummaryRepository.InsertTotalShippingCostByCar(summary.Id, price);

            return newShipping;
        }
    }
}
