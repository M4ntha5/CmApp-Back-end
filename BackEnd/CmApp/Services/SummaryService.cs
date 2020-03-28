using CmApp.Contracts;
using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class SummaryService : ISummaryService
    {
        public ISummaryRepository SummaryRepository { get; set; }

        public async Task DeleteSummary(string carId, string summaryId)
        {
            await SummaryRepository.DeleteCarSummary(carId, summaryId);
        }

        public async Task UpdateSoldSummary(string carId, SummaryEntity summary)
        {
            summary.Car = carId;
            var oldSummary = await GetSummaryByCarId(carId);
            summary.Id = oldSummary.Id;
            await SummaryRepository.UpdateCarSoldSummary(summary);
        }

        public async Task<SummaryEntity> GetSummaryByCarId(string carId)
        {
            var summary = await SummaryRepository.GetSummaryByCarId(carId);

            return summary;
        }

        public async Task<SummaryEntity> InsertCarSummary(string carId, SummaryEntity summary)
        {
            summary.Car = carId;
            var newSummary = await SummaryRepository.InsertSummary(summary);
            return newSummary;
        }
    }
}
