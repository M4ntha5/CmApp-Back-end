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
using System.IO;
using System.Reflection;
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

        public async Task DeleteCarTracking(string carId)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);

            var filter = Builders<TrackingEntity>.Filter.Eq("car", BsonObjectId.Create(carId));

            await repo.DeleteOneAsync(filter);
        }

        public async Task UpdateCarTracking(string trackingId, TrackingEntity tracking)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);
            tracking.Id = trackingId;

            await repo.ReplaceOneAsync(
                x => x.Id == trackingId,
                tracking,
                new DatabaseReplaceOneOptions()
            );
        }
        public async Task<TrackingEntity> GetTrackingByCar(string carId)
        {
            var repo = new CodeMashRepository<TrackingEntity>(Client);

            var filter = Builders<TrackingEntity>.Filter.Eq("car", BsonObjectId.Create(carId));

            var response = await repo.FindOneAsync(filter, new DatabaseFindOneOptions());
            return response;
        }


    }
}
