using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
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

        public async Task UpdateSoldSummary(int carId, SummaryEntity summary)
        {
           // summary.Car = carId;
            summary.SoldDate = DateTime.UtcNow;
            var time = summary.SoldDate.Subtract(summary.CreatedAt);
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
            summary.ID = oldSummary.ID;
            await SummaryRepository.UpdateCarSoldSummary(summary);
        }
        public async Task<SummaryEntity> InsertCarSummary(int carId, SummaryEntity summary)
        {
            if (summary.SelectedCurrency == "" || summary.BaseCurrency == "")
                throw new BusinessException("Currency not set");

           // summary.Car = carId;
            summary.Total = summary.BoughtPrice;
            if (summary.SelectedCurrency != summary.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = summary.BoughtPrice,
                    From = summary.SelectedCurrency,
                    To = summary.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                summary.Total = Math.Round(convertedPrice, 2);
                summary.BoughtPrice = Math.Round(convertedPrice, 2);
            }

            var newSummary = await SummaryRepository.InsertSummary(summary);
            return newSummary;
        }
    }
}
