
using CmApp.Entities;
using Isidos.CodeMash.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface ITrackingRepository
    {
        Task<TrackingEntity> InsertTracking(TrackingEntity tracking);
        Task<UploadRecordFileResponse> UploadImageToTracking(string recordId, byte[] bytes, string imgName);
        Task DeleteCarTracking(string carId, string trackingId);
        Task UpdateCarTracking(string trackingId, TrackingEntity tracking);
        Task<TrackingEntity> GetCarTracking(string carId, string trackingId);

    }
}
