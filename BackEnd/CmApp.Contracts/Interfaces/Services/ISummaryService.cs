using CmApp.Contracts.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface ISummaryService
    {
        Task UpdateSoldSummary(int carId, SummaryEntity summary);
        Task<SummaryEntity> InsertCarSummary(int carId, SummaryEntity summary);
    }
}
