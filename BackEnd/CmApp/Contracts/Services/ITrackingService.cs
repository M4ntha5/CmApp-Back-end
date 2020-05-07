using CmApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ITrackingService
    {
        Task UpdateTracking(string carId, TrackingEntity tracking);
        Task<TrackingEntity> LookForTrackingData(string carId);
        Task<List<string>> LookForTrackingImages(string carId);
        Task DownloadTrackingImages(string carId, List<string> urls);
        Task SaveLastShowImagesStatus(string carId, bool status);
    }
}
