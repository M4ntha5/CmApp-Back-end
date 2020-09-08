using CmApp.Contracts.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface ISummaryRepository
    {
        Task<SummaryEntity> InsertSummary(SummaryEntity summary);
        Task<SummaryEntity> GetSummaryByCarId(int carId);
        Task DeleteCarSummary(int carId);
        Task UpdateCarSoldSummary(SummaryEntity summary);
        Task InsertTotalByCar(int summaryId, double total);
    }
}