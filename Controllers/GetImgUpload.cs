using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using FileUploadApplication.Entities;
using FileUploadApplication.Data;
using Microsoft.EntityFrameworkCore;


namespace FileUploadApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetImgUploadController : ControllerBase
    {
        private readonly DataContext _context;

        public GetImgUploadController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImgData>> GetImgUpload(int id)
        {
            try
            {
                var upload = await _context.ImgDatas.FindAsync(id);

                if(upload!=null && upload.ImageData!=null)
                {
                    byte[] imageData = Convert.FromBase64String(upload.ImageData);
                    var imageStream = new MemoryStream(imageData);
                    return File(imageStream, "image/jpeg");
                    // Return image
                    //var videoStream = new MemoryStream(videoData);
                }
                else
                {
                    return NotFound("Specific Data not found");
                }
            }
            catch (Exception ex)
            {
                return NotFound("Oops something went wrong try again " + ex);
            }
        }
    }
}
