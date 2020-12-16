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
    public class TrackingRepository : ITrackingRepository
    {
        /* private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);

         public async Task<TrackingEntity> InsertTracking(TrackingEntity tracking)
         {
             if (tracking == null)
                 throw new ArgumentNullException(nameof(tracking), "Cannot insert tracking in db, because tracking is empty");

             var repo = new CodeMashRepository<TrackingEntity>(Client);

             var response = await repo.InsertOneAsync(tracking, new DatabaseInsertOneOptions());
             return response;
         }
         public async Task<List<string>> UploadImageToTracking(string recordId, List<string> urls)
         {
             var repo = new CodeMashRepository<TrackingEntity>(Client);

             var entity = new List<Urls>();

             urls.ForEach(x => entity.Add(new Urls { Url = x }));

             var update = Builders<TrackingEntity>.Update.Set("images", entity);

             var response = await repo.UpdateOneAsync(recordId, update, new DatabaseUpdateOneOptions());
             return urls;
         }

         public async Task DeleteTrackingImages(string recordId)
         {
             var repo = new CodeMashRepository<TrackingEntity>(Client);
             var update = Builders<TrackingEntity>.Update.Set("images", new List<Urls>());
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
             await repo.UpdateOneAsync(trackingId, update, new DatabaseUpdateOneOptions());
         }*/

        private readonly Context _context;

        public TrackingRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteCarTracking(int carId)
        {
            var tracking = await _context.Trackings.FirstOrDefaultAsync(x => x.CarId == carId);
            if(tracking != null)
            {
                _context.Trackings.Remove(tracking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTrackingImages(int trackingId)
        {
            var tracking = await _context.Trackings
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == trackingId);
            if(tracking != null)
            {
                tracking.Images = null;
                await _context.SaveChangesAsync();
            }
        }

        public Task<Tracking> GetTrackingByCar(int carId)
        {
            return _context.Trackings.FirstOrDefaultAsync(x => x.CarId == carId);
        }

        public async Task<Tracking> InsertTracking(Tracking tracking)
        {
            if (tracking == null)
                throw new ArgumentNullException(nameof(tracking), "Cannot insert tracking in db, because tracking is empty");

            await _context.Trackings.AddAsync(tracking);
            await _context.SaveChangesAsync();
            return tracking;
        }

        public async Task UpdateCarTracking(int trackingId, Tracking newTracking)
        {
            var tracking = await _context.Trackings.FirstOrDefaultAsync(x => x.Id == trackingId);
            if(tracking != null)
            {
                tracking.ContainerNumber = newTracking.ContainerNumber;
                tracking.BookingNumber = newTracking.BookingNumber;
                tracking.Vin = newTracking.Vin;
                tracking.Year = newTracking.Year;
                tracking.Make = newTracking.Make;
                tracking.Model = newTracking.Model;
                tracking.Title = newTracking.Title;
                tracking.State = newTracking.State;
                tracking.Status = newTracking.Status;
                tracking.DateReceived = newTracking.DateReceived;
                tracking.DateOrdered = newTracking.DateOrdered;
                tracking.Branch = newTracking.Branch;
                tracking.ShippingLine = newTracking.ShippingLine;
                tracking.FinalPort = newTracking.FinalPort;
                tracking.DatePickedUp = newTracking.DatePickedUp;
                tracking.Color = newTracking.Color;
                tracking.Damage = newTracking.Damage;
                tracking.Condition = newTracking.Condition;
                tracking.Keys = newTracking.Keys;
                tracking.Running = newTracking.Running;
                tracking.Wheels = newTracking.Wheels;
                tracking.AirBag = newTracking.AirBag;
                tracking.Radio = newTracking.Radio;
                await _context.SaveChangesAsync();
            }
        }

        public Task UpdateImageShowStatus(int trackingId, bool status)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> UploadImageToTracking(int trackingId, List<string> urls)
        {
            throw new NotImplementedException();
            /*var tracking = await _context.Trackings.FirstOrDefaultAsync(x => x.Id == trackingId);
            if(tracking != null)
            {
                var entity = new List<ImageUrl>();

                urls.ForEach(x => entity.Add(new ImageUrl { Url = x }));
                tracking.Images = urls;

                await _context.SaveChangesAsync();
            }*/
            
        }
    }
}
