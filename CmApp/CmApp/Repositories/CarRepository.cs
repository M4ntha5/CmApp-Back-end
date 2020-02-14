using CmApp.Contracts;
using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class CarRepository : ICarRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<CarEntity> InsertCar(CarEntity car)
        {
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car), "Cannot insert car in db, because car is empty");
            }

            var repo = new CodeMashRepository<CarEntity>(Client);

            var entity = new CarEntity
            {
                Parameters = car.Parameters,
                Equipment = car.Equipment
            };
            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response;
        }

        public async Task<List<CarEntity>> GetAllCars()
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var cars = await repo.FindAsync(x => true, new DatabaseFindOptions()
            {
                PageNumber = 0,
                PageSize = 100
            });

            return cars.Items;
        }

        public async Task<CarEntity> GetCarById(string id)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var car = await repo.FindOneAsync(x => x.Id == id, new DatabaseFindOneOptions());

            return car;
        }
        public async Task UpdateCar(CarEntity car)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            await repo.ReplaceOneAsync(
                x => x.Id == car.Id,
                car,
                new DatabaseReplaceOneOptions()
            );

        }

        public async Task DeleteCar(CarEntity car)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            await repo.DeleteOneAsync(x => x.Id == car.Id);

        }

        public async Task<List<CarEntity>> test()
        {
            var repo = new CarRepository();

            var cars = await repo.GetAllCars();
            return cars;
        }

    }
}
