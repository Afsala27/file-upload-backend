using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace InfrastructureLayer.Interfaces
{
    public interface IGoogleDrive
    {
        Task<string?> UploadFileAsync (IFormFile file, string filename);

        Task<byte[]> DownloadFileAsync(string fileId);

        Task<byte[]> GetImageContentFromDriveAsync(string fileId);

        Task <byte[]> GetVideoContentFromDriveAsync(string fileId);
    }
}