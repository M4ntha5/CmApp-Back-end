using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("/api/cars/{carId}/summary")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly SummaryService summaryService = new SummaryService()
        {
            SummaryRepository = new SummaryRepository()
        };

        // GET: /api/cars/{carId}/summary
        [HttpGet]
        public Task<SummaryEntity> Get(string carId)
        {
            var summary = summaryService.GetSummaryByCarId(carId);
            return summary;
        }

        // POST: /api/cars/{carId}/summary
        [HttpPost]
        public SummaryEntity Post(string carId, [FromBody] SummaryEntity summary)
        {
            var newSummary = summaryService.InsertCarSummary(carId, summary).Result;
            return newSummary;
        }

        // PUT: /api/cars/{carId}/summary/{summaryId}
        [HttpPut("{summaryId}")]
        public async Task<IActionResult> Put(string carId, string summaryId, [FromBody] SummaryEntity summary)
        {
            await summaryService.UpdateSummary(carId, summaryId, summary);
            return NoContent();
        }

        // DELETE: /api/cars/{carId}/summary/{summaryId}
        [HttpDelete("{summaryId}")]
        public async Task<IActionResult> Delete(string carid, string summaryId)
        {
            await summaryService.DeleteSummary(carid, summaryId);
            return NoContent();
        }
    }
}
