using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{

    [ApiController]
    public class CustomController : ControllerBase
    {
        CarRepository repo = new CarRepository();

        // GET: api/Custom
        [Route("api/user-car-names")]
        [HttpGet]
        public async Task<List<object>> GetCarNames()
        {
            var cars = await repo.GetAllCars();
            var result = new List<object>();

            foreach(var car in cars)
                result.Add( new { id = car.Id, name = car.Make + " " + car.Model });

            return result;
        }
    }
}
