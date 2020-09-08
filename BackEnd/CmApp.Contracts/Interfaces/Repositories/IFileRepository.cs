using image4ioDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Repositories
{
    public interface IFileRepository
    {
        byte[] StreamToByteArray(MemoryStream mem);
        Task<Stream> GetFile(string fileId);
        byte[] Base64ToByteArray(string base64String);
        string ByteArrayToBase64String(byte[] bytes);
        Tuple<string, string> GetFileId(object file);
        Task<string> GetFileUrl(string fileId);

        Task<List<string>> InsertCarImages(int carId, List<UploadImageRequest.File> files);
        Task<List<string>> ListFolder(string folder);
        Task<bool> DeleteImage(string pathToImage);
        Task<bool> DeleteFolder(string folder);

        //bring back if needed
        //Task<List<string>> InsertTrackingImages(string carId, List<UploadImageRequest.File> files);

    }
}
