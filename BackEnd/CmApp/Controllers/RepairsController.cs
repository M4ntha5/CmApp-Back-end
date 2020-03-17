using System.Collections.Generic;
using System.Threading.Tasks;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("/api/cars/{carId}/repairs")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        private readonly RepairService repairService = new RepairService
        {
            RepairRepository = new RepairRepository(),
            SummaryRepository = new SummaryRepository()
        };

        // GET: api/cars/{carId}/Repairs
        [HttpGet]
        public List<RepairEntity> Get(string carId)
        {
            var repairs = repairService.GetAllSelectedCarRepairs(carId).Result;
            return repairs;
        }

        // GET: api/cars/{carId}/Repairs/5
        [HttpGet("{repairId}")]
        public RepairEntity Get(string carId, string repairId)
        {
            var repair = repairService.GetSelectedCarRepairById(carId, repairId).Result;
            return repair;
        }

        // POST: api/cars/{carId}/Repairs
        [HttpPost]
        public RepairEntity Post(string carId, [FromBody] RepairEntity repair)
        {
            var newRepair = repairService.InsertCarRepair(carId, repair).Result;
            return newRepair;
        }

        // PUT: api/cars/{carId}/Repairs/5
        [HttpPut("{repairId}")]
        public async Task<IActionResult> Put(string carId, string repairId, [FromBody] RepairEntity repair)
        {
            await repairService.UpdateSelectedCarRepair(repairId, carId, repair);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{repairId}")]
        public async Task<IActionResult> Delete(string carId, string repairId)
        {
            await repairService.DeleteSelectedCarRepair(carId, repairId);
            return NoContent();
        }
    }
}
