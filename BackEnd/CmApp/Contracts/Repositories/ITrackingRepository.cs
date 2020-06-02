using CmApp.Entities;
using Isidos.CodeMash.ServiceContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ITrackingRepository
    {
        Task<TrackingEntity> InsertTracking(TrackingEntity tracking);
        Task<List<string>> UploadImageToTracking(string recordId, List<string> urls);
        Task DeleteCarTracking(string carId);
        Task UpdateCarTracking(string trackingId, TrackingEntity tracking);
        Task<TrackingEntity> GetTrackingByCar(string carId);
        Task DeleteTrackingImages(string recordId);
        Task UpdateImageShowStatus(string trackingId, bool status);

    }
}
