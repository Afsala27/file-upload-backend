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
    public class GetVideoUploadController : ControllerBase
    {
        private readonly DataContext _context;

        public GetVideoUploadController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImgData>> GetVideoUpload(int id)
        {
            try
            {
                var uploads = await _context.ImgDatas.FindAsync(id);

                if(uploads != null && uploads.VideoData != null)
                {
                    byte[] videoData = Convert.FromBase64String(uploads.VideoData);
                    var videoStream = new MemoryStream(videoData);
                    //return File(imageStream, "image/jpeg");
                    // return video
                    return File(videoStream, "video/mp4", $"video_{id}.mp4");//"video.mp4", "inline");
                    //, $"video_{id}.mp4"
                }
                else
                {
                    return NotFound("Specific Data not found");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error: "+ ex.Message);
            }
        }
    }
}
