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
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class TrackingRepository : ITrackingRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

        public async Task<TrackingEntity> InsertTracking(TrackingEntity tracking)
        {
            if (tracking == null)
            {
                throw new ArgumentNullException(nameof(tracking), "Cannot insert tracking in db, because tracking is empty");
            }

            var repo = new CodeMashRepository<TrackingEntity>(Client);

            var response = await repo.InsertOneAsync(tracking, new DatabaseInsertOneOptions());
            return response;
        }
        public async Task<UploadRecordFileResponse> UploadImageToTracking(string recordId, byte[] bytes, string imgName)
        {
            var filesService = new CodeMashFilesService(Client);
 
            var response = await filesService.UploadRecordFileAsync(bytes, imgName,
                new UploadRecordFileRequest
                {
                    RecordId = recordId,
                    CollectionName = "tracking",
                    UniqueFieldName = "auction_photos"
                    
                });
            return response;
        }
        public async Task DeleteTrackingImages(string recordId)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);

            UpdateDefinition<TrackingEntity>[] updates =
            {
                Builders<TrackingEntity>.Update.Set("auction_photos", new List<object>()),
            };

            var update = Builders<TrackingEntity>.Update.Combine(updates);

            await repo.UpdateOneAsync(recordId, update, new DatabaseUpdateOneOptions());
        }

        public async Task DeleteCarTracking(string carId)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);

            var filter = Builders<TrackingEntity>.Filter.Eq("car", BsonObjectId.Create(carId));

            await repo.DeleteOneAsync(filter);
        }

        public async Task UpdateCarTracking(string trackingId, TrackingEntity tracking)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);

            var update = Builders<TrackingEntity>.Update
                .Set("container_number", tracking.ContainerNumber)
                .Set("booking_number", tracking.BookingNumber)
                .Set("url", tracking.Url)
                .Set("vin", tracking.Vin)
                .Set("year", tracking.Year)
                .Set("make", tracking.Make)
                .Set("model", tracking.Model)
                .Set("title", tracking.Title)
                .Set("state", tracking.State)
                .Set("status", tracking.Status)
                .Set("date_received", tracking.DateReceived)
                .Set("order_date", tracking.DateOrdered)
                .Set("branch", tracking.Branch)
                .Set("shipping_line", tracking.ShippingLine)
                .Set("final_port", tracking.FinalPort)
                .Set("pick_up_date", tracking.DatePickedUp)
                .Set("color", tracking.Color)
                .Set("damage", tracking.Damage)
                .Set("condition", tracking.Condition)
                .Set("keys", tracking.Keys)
                .Set("running", tracking.Running)
                .Set("wheels", tracking.Wheels)
                .Set("air_bag", tracking.AirBag)
                .Set("radio", tracking.Radio);

            await repo.UpdateOneAsync(
                trackingId,
                update,
                new DatabaseUpdateOneOptions()
            );

        }
        public async Task<TrackingEntity> GetTrackingByCar(string carId)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);

            var filter = Builders<TrackingEntity>.Filter.Eq("car", BsonObjectId.Create(carId));

            var response = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());
            return response;
        }

        public async Task UpdateImageShowStatus(string trackingId, bool status)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);

            var update = Builders<TrackingEntity>.Update.Set("show_images", status);

            await repo.UpdateOneAsync(trackingId, update, 
                new DatabaseUpdateOneOptions());
        }


    }
}
