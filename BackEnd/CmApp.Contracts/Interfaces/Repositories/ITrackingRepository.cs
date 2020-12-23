using CmApp.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface ITrackingRepository
    {
        Task<Tracking> InsertTracking(Tracking tracking);
        Task<ICollection<TrackingImage>> UploadImageToTracking(int recordId, List<string> urls);
        Task DeleteCarTracking(int carId);
        Task UpdateCarTracking(int trackingId, Tracking tracking);
        Task<Tracking> GetTrackingByCar(int carId);
        Task DeleteTrackingImages(int recordId);
        Task UpdateImageShowStatus(int trackingId, bool status);

    }
}
