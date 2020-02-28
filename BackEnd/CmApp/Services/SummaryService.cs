using CmApp.Contracts;
using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class SummaryService : ISummaryService
    {
        public ISummaryRepository SummaryRepository { get; set; }

        public async Task<int> DeleteSummary(string carId, string summaryId)
        {
            var result = await SummaryRepository.DeleteCarSummary(carId, summaryId);
            return result;
           
        }

        public async Task UpdateSummary(string carId, string summaryId, SummaryEntity summary)
        {
            summary.Car = carId;
            summary.Id = summaryId;
            await SummaryRepository.UpdateCarSummary(summary);
        }

        public async Task<SummaryEntity> GetSummaryByCarId(string carId)
        {
            var car = await SummaryRepository.GetSummaryByCarId(carId);
            return car;
        }

        public async Task<SummaryEntity> InsertCarSummary(string carId, SummaryEntity summary)
        {
            summary.Car = carId;
            var car = await SummaryRepository.InsertSummary(summary);
            return car;
        }
    }
}
