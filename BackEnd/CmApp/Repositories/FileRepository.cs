using CmApp.Contracts;
using CmApp.Utils;
using CodeMash.Client;
using CodeMash.Project.Services;
using Isidos.CodeMash.ServiceContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Repositories
{
    public class FileRepository : IFileRepository
    {
        private static CodeMashClient Client => new CodeMashClient(Settings.ApiKey, Settings.ProjectId);
        
        public Task<Stream> GetFile(string fileId)
        {
            var filesRepo = new CodeMashFilesService(Client);

            var response = filesRepo.GetFileStreamAsync(new GetFileRequest()
            {
                FileId = fileId,
                ProjectId = Settings.ProjectId
            });
            return response;
        }
        public byte[] StreamToByteArray(MemoryStream mem)
        {
            mem.Seek(0, SeekOrigin.Begin);
            int count = 0;
            byte[] byteArray = new byte[mem.Length];
            while (count < mem.Length)
            {
                byteArray[count++] = Convert.ToByte(mem.ReadByte());
            }
            return byteArray;
        }
        public byte[] Base64ToByteArray(string base64String)
        {
            return Convert.FromBase64String(base64String);
        }

        public string ByteArrayToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
