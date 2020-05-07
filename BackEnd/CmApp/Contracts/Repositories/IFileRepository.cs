using System;
using System.IO;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IFileRepository
    {
        byte[] StreamToByteArray(MemoryStream mem);
        Task<Stream> GetFile(string fileId);
        byte[] Base64ToByteArray(string base64String);
        string ByteArrayToBase64String(byte[] bytes);
        Tuple<string, string> GetFileId(object file);
        Task<string> GetFileUrl(string fileId);
    }
}
