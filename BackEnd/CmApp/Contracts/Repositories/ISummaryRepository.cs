using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ISummaryRepository
    {
        Task<SummaryEntity> InsertSummary(SummaryEntity summary);
        Task<SummaryEntity> GetSummaryByCarId(string carId);
        Task DeleteCarSummary(string carId);
        Task UpdateCarSoldSummary(SummaryEntity summary);
        Task InsertTotalByCar(string summaryId, double total);
    }
}
