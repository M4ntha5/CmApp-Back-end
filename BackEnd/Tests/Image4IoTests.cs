using CmApp.Repositories;
using image4ioDotNetSDK.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Tests
{
    class Image4IoTests
    {
        FileRepo repo;
        [SetUp]
        public void Setup()
        {
            repo = new FileRepo();
        }

        [Test]
        public async Task InsertCarImages()
        {
            var carId = "5ed4ef42fca2790004ef90e9";
            FileStream file1 = File.Open(@"C:\Users\Mantas\Desktop\New folder (3)\iCloud Photos\IMG_0505.jpeg", FileMode.Open);
            FileStream file2 = File.Open(@"C:\Users\Mantas\Desktop\New folder (3)\iCloud Photos\IMG_0153.jpeg", FileMode.Open);

            var files = new List<UploadImageRequest.File>()
            {
                new UploadImageRequest.File()
                {
                    Data = file1,
                    FileName = "1.jpeg"
                },
                new UploadImageRequest.File()
                {
                    Data = file2,
                    FileName = "2.jpeg"
                },
            };

            var insertedUrls = await repo.InsertCarImages(carId, files);

            Assert.AreEqual(files.Count, insertedUrls.Count);
        }

        [Test]
        public async Task InsertTrackingImages()
        {
            var carId = "5ed4ef42fca2790004ef90e9";
            FileStream file1 = File.Open(@"C:\Users\Mantas\Desktop\New folder (3)\iCloud Photos\IMG_0505.jpeg", FileMode.Open);
            FileStream file2 = File.Open(@"C:\Users\Mantas\Desktop\New folder (3)\iCloud Photos\IMG_0153.jpeg", FileMode.Open);

            var files = new List<UploadImageRequest.File>()
            {
                new UploadImageRequest.File()
                {
                    Data = file1,
                    FileName = "1.jpeg"
                },
                new UploadImageRequest.File()
                {
                    Data = file2,
                    FileName = "2.jpeg"
                },
            };

            var insertedUrls = await repo.InsertTrackingImages(carId, files);

            Assert.AreEqual(files.Count, insertedUrls.Count);
        }

        [Test]
        public async Task DeleteImage()
        {
            var carId = "5ed4ef42fca2790004ef90e9";
            var path = "/cars/" + carId + "/" + carId + "_2.jpeg";
            var deleted = await repo.DeleteImage(path);
            Assert.IsTrue(deleted);
        }

        [Test]
        public async Task DeleteFolder()
        {
            var carId = "5ed4ef42fca2790004ef90e9";
            var folder = "/cars/" + carId;
            var deleted = await repo.DeleteFolder(folder);
            Assert.IsTrue(deleted);
        }

        [Test]
        public async Task ListFolderContent()
        {
            var carId = "5ed4ef42fca2790004ef90e9";
            var folder = "/cars";
            var urls = await repo.ListFolder(folder);
            Assert.IsNotEmpty(urls);
        }

        [Test]
        public async Task CompressImage()
        {
            await repo.CompressImage();
        }
    }
}
