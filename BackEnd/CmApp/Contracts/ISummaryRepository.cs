using CmApp.Entities;
using CodeMash.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ISummaryRepository
    {
        Task<SummaryEntity> InsertSummary(SummaryEntity summary);
        Task<SummaryEntity> GetSummaryByCarId(string carId);
        Task<int> DeleteCarSummary(string carId, string summaryId);
        Task UpdateCarSummary(SummaryEntity summary);
    }
}
