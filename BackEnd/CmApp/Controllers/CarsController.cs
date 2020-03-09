using System.Collections.Generic;
using System.Threading.Tasks;
using CmApp.Entities;
using CmApp.Repositories;
using CmApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CmApp.Controllers
{
    [Route("/api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarService carService = new CarService()
        {
            CarRepository = new CarRepository(),
            WebScraper = new WebScraper(),
            SummaryRepository = new SummaryRepository(),
            FileRepository = new FileRepository(),
        };

        // GET: api/Cars
       [HttpGet]
        public List<CarEntity> Get()
        {
            var cars = carService.GetAllCars().Result;
            return cars;
        }

        // GET: api/Cars/5
        [Produces("application/json")]
        [HttpGet("{carId}")]
        public CarEntity Get(string carId)
        {
            var car = carService.GetCarById(carId).Result;
            return car;
        }

        // POST: api/Cars
        [HttpPost]
        public CarEntity Post([FromBody] CarEntity car)
        {
            var newCar = carService.InsertCar(car).Result;
            return newCar;
        }

        // PUT: api/Cars/5
        [HttpPut("{carId}")]
        public async Task<IActionResult> Put(string carId, [FromBody] CarEntity car)
        {
            await carService.UpdateCar(carId, car);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{carId}")]
        public async Task<IActionResult> Delete(string carId)
        {
            await carService.DeleteCar(carId);
            return NoContent();
        }
    }
}
