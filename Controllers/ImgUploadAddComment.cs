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
    public class ImgUploadAddCommentController : ControllerBase
    {
        private readonly DataContext _context;

        public ImgUploadAddCommentController(DataContext context)
        {
            _context = context;
        }
     [HttpPut]
        public async Task<ActionResult<List<ImgData>>> UpdateImgUploadComment(ImgData demodata)
        {
            var upload = await _context.ImgDatas.FindAsync(demodata.UserId);
            try
            {
                if(upload is null)
                    return NotFound("Data not found");
                upload.Comment = demodata.Comment;
                await _context.SaveChangesAsync();
                return Ok(await _context.ImgDatas.ToListAsync());
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error: "+ ex.Message);
            }
        }
    }
}
