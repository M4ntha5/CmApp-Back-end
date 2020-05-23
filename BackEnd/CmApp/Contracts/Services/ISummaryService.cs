using CmApp.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ISummaryService
    {
        Task UpdateSoldSummary(string carId, SummaryEntity summary);
        Task<SummaryEntity> InsertCarSummary(string carId, SummaryEntity summary);
    }
}
