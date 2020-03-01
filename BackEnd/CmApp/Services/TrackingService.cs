using CmApp.Contracts;
using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class TrackingService
    {
        public ITrackingRepository TrackingRepository { get; set; }

        public async Task DeleteTracking(string carId, string trackingId)
        {
            await TrackingRepository.DeleteCarTracking(carId, trackingId);
        }
        public async Task UpdateTracking(string strackingId, TrackingEntity tracking)
        {
            await TrackingRepository.UpdateCarTracking(strackingId, tracking);
        }
        public async Task<TrackingEntity> GetTracking(string carId, string trackingId)
        {
            var cars = await TrackingRepository.GetCarTracking(carId, trackingId);
            return cars;
        }
    }
}
