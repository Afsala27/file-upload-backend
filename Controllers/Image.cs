using Microsoft.AspNetCore.Mvc;
using FileUploadApplication.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using FileUploadApplication.Data;
using FileUploadApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUploadApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageFilesController : ControllerBase
    {
        private readonly GoogleDriveService _googleDriveService;

         private readonly DataContext _context;

        public ImageFilesController(GoogleDriveService googleDriveService, DataContext context)
        {
            _context = context;
            _googleDriveService = googleDriveService;
        }

        [HttpPost("uploadimage")]
        public async Task<ActionResult<List<ImgData>>> ImageUpload(
            //[FromBody] UserData userData, // Expecting user ID from the request body
            [FromForm] ImgData imgData,
            IFormFile imgFile
        )
        {
            if (imgFile == null || imgFile.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                string fileExtension = Path.GetExtension(imgFile.FileName).ToLowerInvariant();
                List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file type. Only images (.jpg, .jpeg, .png, .gif) are allowed.");
                }

                // Generate a unique filename
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imgFile.FileName);

                // Save the file to Google Drive
                var fileId = await _googleDriveService.UploadFileAsync(imgFile, uniqueFileName);

                // Store the file name (or file ID) in the database along with other data
                imgData.ImageData = uniqueFileName; // Store the unique file name in the database
                imgData.DriveFileId = fileId;
                _context.ImgDatas.Add(imgData);
                await _context.SaveChangesAsync();

                return Ok($"File uploaded successfully. File ID: {fileId}");
            }
                catch (Exception ex)
            {
                // Log the error including inner exception details
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, "An error occurred while saving entity changes. See the inner exception for details.");
            }
        }


        // [HttpPost("upload")]
        // public async Task<IActionResult> UploadFile(IFormFile file)
        // {
        //     if (file == null || file.Length == 0)
        //         return BadRequest("No file uploaded.");

        //     try
        //     {
        //         // Use GoogleDriveService to upload the file
        //         var fileId = await _googleDriveService.UploadFileAsync(file);

        //         return Ok($"File uploaded successfully. File ID: {fileId}");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"An error occurred: {ex.Message}");
        //     }
        // }

        [HttpGet("downloadimage/{fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            try
            {
                var fileContent = await _googleDriveService.DownloadFileAsync(fileId);
                return File(fileContent, "application/octet-stream", "downloaded_file.png");
            }
            catch (Exception ex)
            {
                // Handle error
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
