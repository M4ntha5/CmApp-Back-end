using CmApp.Contracts.DTO;
using CmApp.Contracts.DTO.v2;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class SummaryService : ISummaryService
    {
     /*   private readonly ISummaryRepository SummaryRepository;
        private readonly IExternalAPIService ExternalAPIService;

        public SummaryService(ISummaryRepository summaryRepository, IExternalAPIService externalAPIService)
        {
            SummaryRepository = summaryRepository;
            ExternalAPIService = externalAPIService;
        }

        public async Task InsertCarSummary(int carId, SummaryDTO summary)
        {
            decimal convertedPrice = summary.BoughtPrice;
            if (summary.BoughtPriceCurrency != summary.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = summary.BoughtPrice,
                    From = summary.BoughtPriceCurrency,
                    To = summary.BaseCurrency
                };
                convertedPrice = await ExternalAPIService.CalculateResult(input);
            }
            var summaryEntity = new Summary
            {
                CreatedAt = DateTime.Now,
                BoughtPrice = Math.Round(convertedPrice, 2),
                CarId = carId,
                IsSold = false,
            };

            await SummaryRepository.InsertSummary(summaryEntity);
        }





        public async Task UpdateSoldSummary(int carId, Summary summary)
        {
            // summary.Car = carId;
            summary.SoldDate = DateTime.UtcNow;
            var time = summary.SoldDate.Value.Subtract(summary.CreatedAt);
            string message;
            if (time.Days > 0)
                if (time.Days == 1)
                    message = $"Sold within {time.Days} day";
                else
                    message = $"Sold within {time.Days} days";
            else
                if (time.Hours == 1)
                message = $"Sold within {time.Hours} hour";
            else
                message = $"Sold within {time.Hours} hours";

            summary.SoldWithin = message;
            var oldSummary = await SummaryRepository.GetSummaryByCarId(carId);
            await SummaryRepository.UpdateCarSoldSummary(oldSummary.Id, summary);
        }
        */
    }
}
