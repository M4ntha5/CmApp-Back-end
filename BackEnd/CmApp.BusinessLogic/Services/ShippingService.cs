using CmApp.Contracts;
using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IExternalAPIService ExternalAPIService;
        private readonly IShippingRepository ShippingRepository;
        private readonly ISummaryRepository SummaryRepository;

        public ShippingService(IExternalAPIService externalAPIService, IShippingRepository shippingRepository, 
            ISummaryRepository summaryRepository)
        {
            ExternalAPIService = externalAPIService;
            ShippingRepository = shippingRepository;
            SummaryRepository = summaryRepository;
        }


        public async Task UpdateShipping(int carId, Shipping shipping)
        {
            if (string.IsNullOrEmpty(shipping.AuctionFeeCurrency) ||
                string.IsNullOrEmpty(shipping.CustomsCurrency) ||
                string.IsNullOrEmpty(shipping.TransportationFeeCurrency) ||
                string.IsNullOrEmpty(shipping.TransferFeeCurrency))
                throw new BusinessException("Shipping data currencies not set");

        // shipping.Car = carId;

            shipping = await ConvertShippingPrices(shipping);

            decimal totalPrice = shipping.Customs.Value + shipping.AuctionFee.Value +
                shipping.TransferFee.Value + shipping.TransportationFee.Value;

            var oldShipping = await ShippingRepository.GetShippingByCar(carId);
            await ShippingRepository.UpdateCarShipping(oldShipping.Id, shipping);

            totalPrice -= (oldShipping.Customs.Value + oldShipping.AuctionFee.Value +
                oldShipping.TransferFee.Value + oldShipping.TransportationFee.Value);

            var summary = await SummaryRepository.GetSummaryByCarId(carId);
        // await SummaryRepository.InsertTotalByCar(summary.Id, Math.Round(summary.Total + Math.Round(totalPrice, 2), 2));

        }
        public async Task<Shipping> InsertShipping(int carId, Shipping shipping)
        {
            if (string.IsNullOrEmpty(shipping.AuctionFeeCurrency) ||
                string.IsNullOrEmpty(shipping.CustomsCurrency) ||
                string.IsNullOrEmpty(shipping.TransportationFeeCurrency) ||
                string.IsNullOrEmpty(shipping.TransferFeeCurrency))
                throw new BusinessException("Shipping data currencies not set");

        // shipping.Car = carId;

            shipping = await ConvertShippingPrices(shipping);

            var newShipping = await ShippingRepository.InsertShipping(shipping);

            decimal totalPrice = newShipping.Customs.Value + newShipping.AuctionFee.Value +
                newShipping.TransferFee.Value + newShipping.TransportationFee.Value;

            var summary = await SummaryRepository.GetSummaryByCarId(carId);
            //await SummaryRepository.InsertTotalByCar(summary.ID, Math.Round(summary.Total + totalPrice, 2));

            return newShipping;
        }

        private async Task<Shipping> ConvertShippingPrices(Shipping shipping)
        {
            if (shipping.TransferFeeCurrency != "EUR")//shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.TransferFee.Value,
                    From = shipping.TransferFeeCurrency,
                    To = "EUR"//shipping.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                shipping.TransferFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.TransportationFeeCurrency != "EUR") //shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.TransportationFee.Value,
                    From = shipping.TransportationFeeCurrency,
                    To = "EUR"//shipping.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                shipping.TransportationFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.AuctionFeeCurrency != "EUR") //shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.AuctionFee.Value,
                    From = shipping.AuctionFeeCurrency,
                    To = "EUR"//shipping.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                shipping.AuctionFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.CustomsCurrency != "EUR") //shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.Customs.Value,
                    From = shipping.CustomsCurrency,
                    To = "EUR"//shipping.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                shipping.Customs = Math.Round(convertedPrice, 2);
            }

            return shipping;
        }

    }
}
