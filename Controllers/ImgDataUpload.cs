using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using FileUploadApplication.Entities;
using FileUploadApplication.Data;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace FileUploadApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImgDataUploadController : ControllerBase
    {
        private readonly DataContext _context;

        public ImgDataUploadController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<ImgData>>> ImgUpload(
        [FromForm] ImgData imgData,
        IFormFile imageFile
        //UserData userData
        //[FromBody] UserData userData // Expecting user ID from the request body
        )
        {
            //imgData.UserId = userData.UserId;
            //imgData.UserId = 1; // need to get the userid from the login api

            try
            {
                if (imageFile!=null)
                {
                    string fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                    List<string> allowedExtions = new List<string> {".jpg", ".jpeg", ".png", ".gif" };

                    if(!allowedExtions.Contains(fileExtension))
                        {
                            return BadRequest("Invalid file type. Only images (.jpg, .jpeg, .png, .gif) are allowed.");
                        }

                    imgData.ImageData = await ReadFileContent(imageFile);
                }

                var existingfileI = await _context.ImgDatas.FirstOrDefaultAsync(u => u.ImageData == imgData.ImageData);
                if (existingfileI != null)
                    {
                        return Conflict("This Data is already exists.");
                    }

                _context.ImgDatas.Add(imgData);
                await _context.SaveChangesAsync();

                return Ok(await _context.ImgDatas.ToListAsync());
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error: "+ex.Message);
            }

        }

        private async Task<string> ReadFileContent(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }


    }
}
