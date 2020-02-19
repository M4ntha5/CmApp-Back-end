using CmApp.Contracts;
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
using System.IO;
using System.Reflection;
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
                ManufactureDate = car.ManufactureDate,
                Engine = car.Engine,
                Interior = car.Interior,
                Make = car.Make,
                Model = car.Model,
                Power = car.Power,
                Series = car.Series,
                Steering = car.Steering,
                Transmission = car.Transmission,
                Equipment = car.Equipment,
                Images = new List<object>(),
                Vin = car.Vin
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
        public async Task UpdateCar(string id, CarEntity car)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);
            car.Id = id;
            await repo.ReplaceOneAsync(
                x => x.Id == id,
                car,
                new DatabaseReplaceOneOptions()
            );
        }

        public async Task DeleteCar(CarEntity car)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            await repo.DeleteOneAsync(x => x.Id == car.Id);

        }

        public async Task<UploadRecordFileResponse> UploadImage(string recordId, string fileName)
        {
            var filesService = new CodeMashFilesService(Client);

            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{directory}\\{fileName}";

            using var fsSource = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var response = await filesService.UploadRecordFileAsync(fsSource, recordId+"_image.jpg",
                new UploadRecordFileRequest
                {
                CollectionName = "cars",
                UniqueFieldName = "images",
                RecordId = recordId
                });
            return response;
        }

       /* public async Task AddImageToCar(string carId, Image img)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var result = await repo.UpdateOneAsync(x => x.Id == carId,
                       Builders<CarEntity>.Update.AddToSet($"images", img), null);

        }*/

        // bandymas su controleriu
        public async Task<List<CarEntity>> test()
        {
            var repo = new CarRepository();

            var cars = await repo.GetAllCars();
            return cars;
        }



    }
}
