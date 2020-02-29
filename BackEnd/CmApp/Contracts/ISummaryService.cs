using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ISummaryService
    {
        Task DeleteSummary(string carId, string summaryId);
        Task UpdateSummary(string carId, string summaryId, SummaryEntity summary);
        Task<SummaryEntity> GetSummaryByCarId(string carId);
        Task<SummaryEntity> InsertCarSummary(string carId, SummaryEntity summary);

    }
}
