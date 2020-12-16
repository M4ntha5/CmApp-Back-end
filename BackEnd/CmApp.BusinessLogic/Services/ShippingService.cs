using CmApp.Contracts;
using CmApp.Contracts.Domains;
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

        public Task<Shipping> InsertShipping(int carId, Shipping shipping)
        {
            throw new NotImplementedException();
        }

        public Task UpdateShipping(int carId, Shipping shipping)
        {
            throw new NotImplementedException();
        }

        /* public async Task UpdateShipping(int carId, Shipping shipping)
         {
             if (string.IsNullOrEmpty(shipping.AuctionFeeCurrency) ||
                 string.IsNullOrEmpty(shipping.CustomsCurrency) ||
                 string.IsNullOrEmpty(shipping.TransportationFeeCurrency) ||
                 string.IsNullOrEmpty(shipping.TransferFeeCurrency))
                 throw new BusinessException("Shipping data currencies not set");

            // shipping.Car = carId;

             shipping = await ConvertShippingPrices(shipping);

             double totalPrice = shipping.Customs + shipping.AuctionFee +
                 shipping.TransferFee + shipping.TransportationFee;

             var oldShipping = await ShippingRepository.GetShippingByCar(carId);
             await ShippingRepository.UpdateCarShipping(oldShipping.ID, shipping);

             totalPrice -= (oldShipping.Customs + oldShipping.AuctionFee +
                 oldShipping.TransferFee + oldShipping.TransportationFee);

             var summary = await SummaryRepository.GetSummaryByCarId(carId);
             await SummaryRepository.InsertTotalByCar(summary.ID, Math.Round(summary.Total + Math.Round(totalPrice, 2), 2));

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

             double totalPrice = newShipping.Customs + newShipping.AuctionFee +
                 newShipping.TransferFee + newShipping.TransportationFee;

             var summary = await SummaryRepository.GetSummaryByCarId(carId);
             await SummaryRepository.InsertTotalByCar(summary.ID, Math.Round(summary.Total + totalPrice, 2));

             return newShipping;
         }

         private async Task<Shipping> ConvertShippingPrices(Shipping shipping)
         {
             if (shipping.TransferFeeCurrency != shipping.BaseCurrency)
             {
                 var input = new ExchangeInput
                 {
                     Amount = shipping.TransferFee,
                     From = shipping.TransferFeeCurrency,
                     To = shipping.BaseCurrency
                 };
                 var convertedPrice = await ExternalAPIService.CalculateResult(input);
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
                 var convertedPrice = await ExternalAPIService.CalculateResult(input);
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
                 var convertedPrice = await ExternalAPIService.CalculateResult(input);
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
                 var convertedPrice = await ExternalAPIService.CalculateResult(input);
                 shipping.Customs = Math.Round(convertedPrice, 2);
             }

             return shipping;
         }*/

    }
}
