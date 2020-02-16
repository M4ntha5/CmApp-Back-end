using System.Collections.Generic;
using System.Threading.Tasks;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarService carService = new CarService()
        {
            CarRepository = new CarRepository(),
            WebScraper = new WebScraper()
        };

        // GET: api/Cars
        [HttpGet]
        public List<CarEntity> Get()
        {
            var cars = carService.GetAllCars().Result;
            return cars;
        }

        // GET: api/Cars/5
        [HttpGet("{id}", Name = "Get")]
        public CarEntity Get(string id)
        {
            var car = carService.GetCarById(id).Result;
            return car;
        }

        // POST: api/Cars
        [HttpPost]
        public CarEntity Post([FromBody] CarEntity car)
        {
            var newCar = carService.InsertCarDetailsFromScraper(car).Result;
            return newCar;
        }

        // PUT: api/Cars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CarEntity car)
        {
            await carService.UpdateCar(id, car);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await carService.DeleteCar(id);
            return NoContent();
        }
    }
}
