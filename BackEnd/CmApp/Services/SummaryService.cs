using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using System;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class SummaryService : ISummaryService
    {
        public ISummaryRepository SummaryRepository { get; set; }
        public IExchangeService ExchangeRepository { get; set; }

        public async Task UpdateSoldSummary(string carId, SummaryEntity summary)
        {
            summary.Car = carId;
            summary.SoldDate = DateTime.Now;
            var time = summary.SoldDate.Subtract(summary.CreatedAt);
            string message;
            if (time.Days > 0)
                if(time.Days == 1)
                    message = $"Sold within {time.Days} day";
                else
                    message = $"Sold within {time.Days} days";
            else
                if(time.Hours == 1)
                    message = $"Sold within {time.Hours} hour";
                else
                    message = $"Sold within {time.Hours} hours";

            summary.SoldWithin = message;
            var oldSummary = await SummaryRepository.GetSummaryByCarId(carId);
            summary.Id = oldSummary.Id;
            await SummaryRepository.UpdateCarSoldSummary(summary);
        }

        public async Task<SummaryEntity> InsertCarSummary(string carId, SummaryEntity summary)
        {
            if (summary.SelectedCurrency == "" || summary.BaseCurrency == "")
                throw new BusinessException("Currency not set");

            summary.Car = carId;
            summary.Total = summary.BoughtPrice;
            if (summary.SelectedCurrency != summary.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = summary.BoughtPrice,
                    From = summary.SelectedCurrency,
                    To = summary.BaseCurrency
                };
                var convertedPrice = await ExchangeRepository.CalculateResult(input);           
                summary.Total = Math.Round(convertedPrice, 2);
                summary.BoughtPrice = Math.Round(convertedPrice, 2);
            }

            var newSummary = await SummaryRepository.InsertSummary(summary);
            return newSummary;
        }
    }
}
