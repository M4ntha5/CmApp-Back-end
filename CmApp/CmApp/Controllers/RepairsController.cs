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
    [Route("/api/cars/{carId}/repairs")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        private readonly RepairService repairService = new RepairService
        {
            RepairRepository = new RepairRepository()
        };

        // GET: api/Repairs
        [HttpGet]
        public List<RepairEntity> Get(string carId)
        {
            var repairs = repairService.GetAllCarRepairs(carId).Result;// (carId).Result;
            return repairs;
        }

        // GET: api/Repairs/5
        [HttpGet("{id}")]
        public string Get(string carId, string id)
        {
            return "value";
        }

        // POST: api/Repairs
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Repairs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
