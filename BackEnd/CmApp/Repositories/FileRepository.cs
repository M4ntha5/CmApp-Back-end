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
    public class FileRepository
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
    }
}
