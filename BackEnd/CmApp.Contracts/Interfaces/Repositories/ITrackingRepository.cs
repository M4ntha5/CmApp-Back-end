using CmApp.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface ITrackingRepository
    {
        Task<TrackingEntity> InsertTracking(TrackingEntity tracking);
        Task<List<string>> UploadImageToTracking(int recordId, List<string> urls);
        Task DeleteCarTracking(int carId);
        Task UpdateCarTracking(int trackingId, TrackingEntity tracking);
        Task<TrackingEntity> GetTrackingByCar(int carId);
        Task DeleteTrackingImages(int recordId);
        Task UpdateImageShowStatus(int trackingId, bool status);

    }
}
