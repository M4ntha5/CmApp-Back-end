using CmApp.Contracts.DTO;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Models;
using CmApp.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Repositories
{
    public class CarRepository : ICarRepository
    {
        /*  private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

          public async Task<CarEntity> InsertCar(CarEntity car)
          {
              if (car == null)
                  throw new ArgumentNullException(nameof(car), "Cannot insert car in db, because car is empty");

              var repo = new CodeMashRepository<CarEntity>(Client);

              var entity = new CarEntity
              {
                  BodyType = car.BodyType,
                  Color = car.Color,
                  Displacement = car.Displacement,
                  Drive = car.Drive,
                  ManufactureDate = car.ManufactureDate.Date,
                  Engine = car.Engine,
                  Interior = car.Interior,
                  Make = car.Make,
                  Model = car.Model,
                  Power = car.Power,
                  Series = car.Series,
                  Steering = car.Steering,
                  Transmission = car.Transmission,
                  Equipment = car.Equipment,
                  Vin = car.Vin,
                  User = car.User,
                  DateCreated = DateTime.Now,
              };
              var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
              return response;
          }

          public async Task<List<CarEntity>> GetAllCars()
          {
              var repo = new CodeMashRepository<CarEntity>(Client);

              var projection = Builders<CarEntity>.Projection
                  .Include(x => x.Make)
                  .Include(x => x.Model)
                  .Include(x => x.Vin)
                  .Include(x => x.DateCreated)
                  .Include(x => x.ManufactureDate);

              var cars = await repo.FindAsync<CarEntity>(x => true, projection, null,
                  new DatabaseFindOptions());

              return cars.Items;
          }
          public async Task<List<CarDisplay>> GetAllUserCars(string userId)
          {
              var repo = new CodeMashRepository<CarEntity>(Client);

              var projection = Builders<CarEntity>.Projection
                  .Include(x => x.Make)
                  .Include(x => x.Model)
                  .Include(x => x.User)
                  .Include(x => x.Vin)
                  .Include(x => x.Urls);

              var filter = Builders<CarEntity>.Filter.Eq("user", ObjectId.Parse(userId));

              var cars = await repo.FindAsync<CarDisplay>(filter, projection, null,
                  new DatabaseFindOptions());

              return cars.Items;
          }

          public async Task<CarEntity> GetCarById(string id)
          {
              var repo = new CodeMashRepository<CarEntity>(Client);
              var car = await repo.FindOneAsync(x => x.ID == id, new DatabaseFindOneOptions());
              return car;
          }

          public async Task UpdateCar(string carId, CarEntity car)
          {
              var repo = new CodeMashRepository<CarEntity>(Client);

              var update = Builders<CarEntity>.Update
                  .Set("model", car.Model)
                  .Set("manufacture_date", car.ManufactureDate)
                  .Set("series", car.Series)
                  .Set("body_type", car.BodyType)
                  .Set("steering", car.Steering)
                  .Set("engine", car.Engine)
                  .Set("displacement", car.Displacement)
                  .Set("power", car.Power)
                  .Set("drive", car.Drive)
                  .Set("transmission", car.Transmission)
                  .Set("color", car.Color)
                  .Set("interior", car.Interior)
                  .Set("equipment", car.Equipment);

              await repo.UpdateOneAsync(
                  carId,
                  update,
                  new DatabaseUpdateOneOptions()
              );
          }

          public async Task DeleteCar(string carId)
          {
              var repo = new CodeMashRepository<CarEntity>(Client);
              await repo.DeleteOneAsync(x => x.Id == carId);

          }

          public async Task<List<string>> UploadImageToCar(string recordId, List<string> urls)
          {
              var repo = new CodeMashRepository<CarEntity>(Client);

              var entity = new List<Urls>();

              urls.ForEach(x => entity.Add(new Urls { Url = x }));

              var update = Builders<CarEntity>.Update.Set("urls", entity);

              var response = await repo.UpdateOneAsync(recordId, update, new DatabaseUpdateOneOptions());
              return urls;
          }

          public async Task DeleteAllCarImages(string carId)
          {
              var repo = new CodeMashRepository<CarEntity>(Client);
              var update = Builders<CarEntity>.Update.Set("urls", new List<Urls>());
              await repo.UpdateOneAsync(carId, update, new DatabaseUpdateOneOptions());
          }

          public async Task<List<CarMakesEntity>> GetAllMakes()
          {
              var service = new CodeMashRepository<CarMakesEntity>(Client);

              var makes = await service.FindAsync(
                  x => true,
                  new DatabaseFindOptions()
              );
              return makes.Items;
          }

          public async Task<CarMakesEntity> InsertCarMake(CarMakesEntity make)
          {
              if (make == null)
                  throw new ArgumentNullException(nameof(make), "Cannot insert make in db, because make is empty");

              var repo = new CodeMashRepository<CarMakesEntity>(Client);

              var makes = await repo.InsertOneAsync(make, new DatabaseInsertOneOptions());
              return makes;
          }

          public async Task UpdateCarMake(CarMakesEntity make)
          {
              var repo = new CodeMashRepository<CarMakesEntity>(Client);

              var update = Builders<CarMakesEntity>.Update.Set("name", make.Make);

              await repo.UpdateOneAsync(make.Id, update, new DatabaseUpdateOneOptions());
          }
          public async Task DeleteCarMake(string makeId)
          {
              var repo = new CodeMashRepository<CarMakesEntity>(Client);

              await repo.DeleteOneAsync(makeId);
          }*/

        private readonly Context _context;

        public CarRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteCar(int carId)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public Task<List<Car>> GetAllCars()
        {
            return _context.Cars.ToListAsync();
        }

        public Task<List<Car>> GetAllUserCars(int userId)
        {
            return _context.Cars.Where(x => x.UserId == userId).ToListAsync();
        }

        public Task<Car> GetCarById(int carId)
        {
            return _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
        }

        public async Task<Car> InsertCar(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car), "Cannot insert car in db, because car is empty");

            car.Tracking = new Tracking() { CarId = car.Id, Vin = car.Vin.ToUpper() };

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task UpdateCar(int carId, Car newCar)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);
            if (car != null)
            {
                car.Vin = newCar.Vin;
                car.ManufactureDate = newCar.ManufactureDate;
                car.Series = newCar.Series;
                car.BodyType = newCar.BodyType;
                car.Steering = newCar.Steering;
                car.FuelType = newCar.FuelType;
                car.Engine = newCar.Engine;
                car.Displacement = newCar.Displacement;
                car.Power = newCar.Power;
                car.Drive = newCar.Drive;
                car.Transmission = newCar.Transmission;
                car.Color = newCar.Color;
                car.Interior = newCar.Interior;
                car.MakeId = newCar.MakeId;
                car.UserId = newCar.UserId;
                car.CreatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public Task<List<CarStats>> GetCarStats(DateTime dateFrom, DateTime dateTo, string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<List<Car>> GetUserCars(int userId)
        {
            return _context.Cars.Where(x => x.UserId == userId).ToListAsync();
        }


        public Task<List<string>> UploadImageToCar(int recordId, List<string> urls)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAllCarImages(int recordId)
        {
            throw new NotImplementedException();
        }
    }
}
