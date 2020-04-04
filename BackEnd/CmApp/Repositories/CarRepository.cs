using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Project.Services;
using CodeMash.Repository;
using Isidos.CodeMash.ServiceContracts;
using MongoDB.Bson;
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
                User = car.User
            };
            var response = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            return response;
        }

        public async Task<List<CarEntity>> GetAllCars()
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var projection = Builders<CarEntity>.Projection
                .Include(x => x.Images)
                .Include(x => x.Make)
                .Include(x => x.Model)
                .Include(x => x.Vin);

            var cars = await repo.FindAsync<CarEntity>(x => true, projection, null,
                new DatabaseFindOptions
                {
                    //PageNumber = 0,
                    //PageSize = 9
                });       

            return cars.Items;
        }
        public async Task<List<CarDisplay>> GetAllUserCars(string userId)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var projection = Builders<CarEntity>.Projection
                .Include(x => x.Images)
                .Include(x => x.Make)
                .Include(x => x.Model)
                .Include(x => x.Vin)
                .Include(x => x.User);


            var filter = Builders<CarEntity>.Filter.Eq("user", ObjectId.Parse(userId));

            var cars = await repo.FindAsync<CarDisplay>(filter, projection, null,
                new DatabaseFindOptions
                {
                    //PageNumber = 0,
                    //PageSize = 9
                });

            return cars.Items;
        }

        public async Task<CarEntity> GetCarById(string id)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var car = await repo.FindOneAsync(x => x.Id == id, new DatabaseFindOneOptions());

            return car;
        }

        public async Task<CarEntity> GetCarByVin(string vin)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var car = await repo.FindOneAsync(x => x.Vin == vin, new DatabaseFindOneOptions());

            return car;
        }
        public async Task UpdateCar(string id, CarEntity car)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var update = Builders<CarEntity>.Update
                .Set("make", car.Make)
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

            _ = await repo.UpdateOneAsync(
                car.Id,
                update,
                new DatabaseUpdateOneOptions()
            );
        }

        public async Task DeleteCar(string carId)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            await repo.DeleteOneAsync(x => x.Id == carId);

        }

        public async Task<UploadRecordFileResponse> UploadImageToCar(string recordId, byte[] bytes, string imgName)
        {
            var filesService = new CodeMashFilesService(Client);

            var response = await filesService.UploadRecordFileAsync(bytes, imgName,
                new UploadRecordFileRequest
                {
                    RecordId = recordId,
                    CollectionName = "cars",
                    UniqueFieldName = "images",                  
                });
            return response;
        }

        public async Task<List<CarMakes>> GetAllMakes()
        {
            var service = new CodeMashRepository<CarMakes>(Client);

            var makes = await service.FindAsync(
                x => true,
                new DatabaseFindOptions()
            );
            return makes.Items;
        }
    }
}
