using CmApp.Contracts.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface ISummaryService
    {
        Task UpdateSoldSummary(int carId, Summary summary);
        Task<Summary> InsertCarSummary(int carId, Summary summary);
    }
}
