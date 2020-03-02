using CmApp.Contracts;
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class TrackingService : ITrackingService
    {
        public ITrackingRepository TrackingRepository { get; set; }

        public async Task DeleteTracking(string carId)
        {
            await TrackingRepository.DeleteCarTracking(carId);
        }
        public async Task UpdateTracking(string carId, TrackingEntity tracking)
        {
            tracking.Car = carId;
            var trackingId = TrackingRepository.GetTrackingByCar(carId).Result.Id;
            await TrackingRepository.UpdateCarTracking(trackingId, tracking);
        }
        public async Task<TrackingEntity> GetTracking(string carId)
        {
            var tracking = await TrackingRepository.GetTrackingByCar(carId);
            return tracking;
        }
        public async Task<TrackingEntity> InsertTracking(string carId, TrackingEntity tracking)
        {
            tracking.Car = carId;
            var newTracking = await TrackingRepository.InsertTracking(tracking);
            return newTracking;
        }
    }
}
