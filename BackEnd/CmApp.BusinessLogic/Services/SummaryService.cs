using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using CmApp.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly ISummaryRepository SummaryRepository;
        private readonly IExternalAPIService ExternalAPIService;

        public SummaryService(ISummaryRepository summaryRepository, IExternalAPIService externalAPIService)
        {
            SummaryRepository = summaryRepository;
            ExternalAPIService = externalAPIService;
        }

        public async Task UpdateSoldSummary(int carId, Summary summary)
        {
            // summary.Car = carId;
            summary.SoldDate = DateTime.UtcNow;
            var time = summary.SoldDate.Value.Subtract(summary.CreatedAt);
            /*string message;
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

            summary.SoldWithin = message;*/
            var oldSummary = await SummaryRepository.GetSummaryByCarId(carId);
            await SummaryRepository.UpdateCarSoldSummary(oldSummary.Id, summary);
        }
        public async Task<Summary> InsertCarSummary(int carId, Summary summary)
        {
            /*if (summary.SelectedCurrency == "" || summary.BaseCurrency == "")
                throw new BusinessException("Currency not set");*/

            // summary.Car = carId;
            //if (summary.SelectedCurrency != summary.BaseCurrency)
            //{
                var input = new ExchangeInput
                {
                    Amount = summary.BoughtPrice,
                    From = "EUR", //summary.SelectedCurrency,
                    To = "EUR", //summary.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                summary.BoughtPrice = (decimal)Math.Round(convertedPrice, 2);
            //}

            var newSummary = await SummaryRepository.InsertSummary(summary);
            return newSummary;
        }
    }
}
