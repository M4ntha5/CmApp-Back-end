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
                Vin = car.Vin
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
                    //PageNumber = ,
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
           /* var filter = new GetFileRequest { FileId = response.Result.Id };

            var url = await filesService.GetFileUrlAsync(filter);

            var repo = new CodeMashRepository<ImagesEntity>(Client);

            var entity = new ImagesEntity { Url = url.Result };

            var insertedImageId = await repo.InsertOneAsync(entity, new DatabaseInsertOneOptions());
            */
            return response;
        }

      /*  public async Task<UploadRecordFileResponse> UploadImageToCar(string recordId, string fileName)
        {
            var filesService = new CodeMashFilesService(Client);

            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = $"{directory}\\{fileName}";

            using var fsSource = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var response = await filesService.UploadRecordFileAsync(fsSource, recordId+"_image.png",
                new UploadRecordFileRequest
                {
                    CollectionName = "cars",
                    UniqueFieldName = "images",
                    RecordId = recordId
                });
            return response;
        }
        */
       /* public async Task AddImageToCar(string carId, Image img)
        {
            var repo = new CodeMashRepository<CarEntity>(Client);

            var result = await repo.UpdateOneAsync(x => x.Id == carId,
                       Builders<CarEntity>.Update.AddToSet($"images", img), null);

        }*/

    }
}
