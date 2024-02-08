using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace FileUploadApplication.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveService()
        {
            string jsonKeyFilePath = "credentials/my-web-project-413404-f34c52f9c92c.json";

            GoogleCredential credential;
            using (var stream = new FileStream(jsonKeyFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(DriveService.ScopeConstants.Drive);
            }

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "FileUploadApplication"
            });
        }

        public async Task<string?> UploadFileAsync(IFormFile file, string filename)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = filename
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = file.OpenReadStream())
            {
                request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id";
                await request.UploadAsync();
            }

            return request.ResponseBody?.Id;
        }

        public async Task<byte[]> DownloadFileAsync(string fileId)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                var request = _driveService.Files.Get(fileId);
                await request.DownloadAsync(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                // Handle error
                throw ex;
            }
            finally
            {
                stream.Close();
            }
    }
}
}
