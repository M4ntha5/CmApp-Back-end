using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ITrackingService
    {
        Task DeleteTracking(string carId);
        Task UpdateTracking(string carId, TrackingEntity tracking);
        Task<TrackingEntity> GetTracking(string carId);
        Task<TrackingEntity> InsertTracking(string carId, TrackingEntity tracking);
        Task<TrackingEntity> LookForTracking(string carId);
    }
}
