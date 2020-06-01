using CmApp.Utils;
using image4ioDotNetSDK;
using image4ioDotNetSDK.Models;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class FileRepo
    {
        private readonly Image4ioAPI Client = new Image4ioAPI(Settings.Image4IoApiKey, Settings.Image4IoSecret);

        public async Task<List<string>> InsertCarImages(string carId, List<UploadImageRequest.File> files)
        {
            if(files == null || files.Count == 0)
                throw new BusinessException("Cannot add images, because no image provided");

            var response = await Client.UploadImageAsync(
                new UploadImageRequest()
                {
                    Path = "/cars/" + carId,
                    UseFilename = true,
                    Overwrite = false,
                    Files = files
                });

            var urls = response.UploadedFiles.Select(x =>x.Url).ToList();
            return urls;
        }

        public async Task<List<string>> InsertTrackingImages(string carId, List<UploadImageRequest.File> files)
        {
            if(files == null || files.Count == 0)
                throw new BusinessException("Cannot add images, because no image provided");

            var response = await Client.UploadImageAsync(
                new UploadImageRequest()
                {
                    Path = "/trackings/" + carId,
                    UseFilename = true,
                    Overwrite = false,
                    Files = files
                });

            var urls = response.UploadedFiles.Select(x =>x.Url).ToList();
            return urls;
        }

        public async Task<List<string>> ListFolder(string folder)
        {
            var response = await Client.ListFolderAsync(
                new ListFolderRequest()
                {
                    Path = folder
                });

            var urls = response.Images.Select(x =>x.Url).ToList();
            return urls;
        }

        public async Task<bool> DeleteImage(string pathToImage)
        {
            var response = await Client.DeleteImageAsync(
                new DeleteImageRequest()
                {
                    Name = pathToImage,
                });
            return response.Success;
        }

        public async Task<bool> DeleteFolder(string folder)
        {
            var response = await Client.DeleteFolderAsync(
                new DeleteFolderRequest()
                {
                    Path = folder,
                });
            return response.Success;
        }

        //not working rn
        public async Task CompressImage()
        {
            FileStream stream2 = File.Open(@"C:\Users\Mantas\Desktop\New folder (3)\iCloud Photos\IMG_0503.jpeg", FileMode.Open);

            var size = stream2.Length;
            var optimizer = new ImageOptimizer();
            var res = optimizer.Compress(stream2);

            size = stream2.Length;
        }
    }
}
