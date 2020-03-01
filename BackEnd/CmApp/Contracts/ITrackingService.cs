using CmApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ITrackingService
    {
        Task DeleteTracking(string carId, string trackingId);
        Task UpdateTracking(string strackingId, TrackingEntity tracking);
        Task<TrackingEntity> GetTracking(string carId, string trackingId);
    }
}
