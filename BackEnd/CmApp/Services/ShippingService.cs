using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using System;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class ShippingService : IShippingService
    {
        public IShippingRepository ShippingRepository { get; set; }
        public ISummaryRepository SummaryRepository { get; set; }
        public IExchangeService ExchangeRepository { get; set; }

        public async Task DeleteShipping(string carId)
        {
            await ShippingRepository.DeleteCarShipping(carId);
        }
        public async Task UpdateShipping(string carId, ShippingEntity shipping)
        {
            shipping.Car = carId;
            var oldShipping = await ShippingRepository.GetShippingByCar(carId);
            await ShippingRepository.UpdateCarShipping(oldShipping.Id, shipping);
        }
        public async Task<ShippingEntity> GetShipping(string carId)
        {
            var shipping = await ShippingRepository.GetShippingByCar(carId);
            return shipping;
        }
        public async Task<ShippingEntity> InsertShipping(string carId, ShippingEntity shipping)
        {
            if (string.IsNullOrEmpty(shipping.AuctionFeeCurrency) ||
                string.IsNullOrEmpty(shipping.CustomsCurrency) ||
                string.IsNullOrEmpty(shipping.TransportationFeeCurrency) ||
                string.IsNullOrEmpty(shipping.TransferFeeCurrency))
                throw new BusinessException("Shipping data currencies not set");

            shipping.Car = carId;

            if(shipping.TransferFeeCurrency != shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.TransferFee,
                    From = shipping.TransferFeeCurrency,
                    To = shipping.BaseCurrency
                };
                var convertedPrice = await ExchangeRepository.CalculateResult(input);
                shipping.TransferFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.TransportationFeeCurrency != shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.TransportationFee,
                    From = shipping.TransportationFeeCurrency,
                    To = shipping.BaseCurrency
                };
                var convertedPrice = await ExchangeRepository.CalculateResult(input);
                shipping.TransportationFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.AuctionFeeCurrency != shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.AuctionFee,
                    From = shipping.AuctionFeeCurrency,
                    To = shipping.BaseCurrency
                };
                var convertedPrice = await ExchangeRepository.CalculateResult(input);
                shipping.AuctionFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.CustomsCurrency != shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.Customs,
                    From = shipping.CustomsCurrency,
                    To = shipping.BaseCurrency
                };
                var convertedPrice = await ExchangeRepository.CalculateResult(input);
                shipping.Customs = Math.Round(convertedPrice, 2);
            }

            var newShipping = await ShippingRepository.InsertShipping(shipping);
           
            double totalPrice = newShipping.Customs + newShipping.AuctionFee +
                newShipping.TransferFee + newShipping.TransportationFee;


            var summary = await SummaryRepository.GetSummaryByCarId(carId);
            await SummaryRepository.InsertTotalByCar(summary.Id, summary.Total + totalPrice);

            return newShipping;
        }
    }
}
