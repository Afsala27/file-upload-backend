using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using FileUploadApplication.Entities;
using FileUploadApplication.Data;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Drive.v3;

namespace FileUploadApplication.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class VideoUploadController : ControllerBase
{
    private readonly DataContext _context;
    private readonly DriveService _driveService;

    public VideoUploadController(DataContext context, DriveService driveService)
    {
        _context = context;
        _driveService = driveService;
    }

    [HttpPost]
        public async Task<ActionResult<List<ImgData>>> VideoUpload(
        //[FromBody] UserData userData, // Expecting user ID from the request body
        [FromForm] ImgData imgData,
        IFormFile videoFile
        )
        {
            try
            {
                // imgData.UserId = userData.UserId;
                imgData.UserId = 1; // need to get the userid from the login api

                if (videoFile!=null) // validating file type
                {
                    string fileExtension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();
                    List<string> allowedExtions = new List<string> {".mp4", ".mov", ".mkv" };

                    if(!allowedExtions.Contains(fileExtension))
                    {
                        return BadRequest("Invalid file type. Only videos (.mp4, .mov, .mkv) are allowed.");
                    }
                    imgData.VideoData = await ReadFileContent(videoFile);
                }

                var existingfileV = await _context.ImgDatas.FirstOrDefaultAsync(u => u.VideoData == imgData.VideoData);
                if (existingfileV != null) // validating file is already eixist or not
                {
                    return Conflict("This Data is already exists.");
                }

                _context.ImgDatas.Add(imgData);
                await _context.SaveChangesAsync();

                return Ok(await _context.ImgDatas.ToListAsync());

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }

        }

        private async Task<string> ReadFileContent(IFormFile file) // file saving section
        {
            // var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            // var filePath = Path.Combine("wwwroot/uploads", fileName);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }
    }

}


