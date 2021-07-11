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
        private readonly Context _context;

        public TrackingRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteCarTracking(int carId)
        {
            /*var tracking = await _context.Trackings.FirstOrDefaultAsync(x => x.CarId == carId);
            if(tracking != null)
            {
                _context.Trackings.Remove(tracking);
                await _context.SaveChangesAsync();
            }*/
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

        public async Task<ICollection<TrackingImage>> UploadImageToTracking(int trackingId, List<string> urls)
        {
            //throw new NotImplementedException();
            var tracking = await _context.Trackings.FirstOrDefaultAsync(x => x.Id == trackingId);
            if(tracking != null)
            {
                var entity = new List<TrackingImage>();

                urls.ForEach(x => entity.Add(new TrackingImage { Url = x, TrackingId = trackingId }));
                _context.TrackingImages.AddRange(entity);

                await _context.SaveChangesAsync();
            }
            return tracking.Images;
        }
    }
}
