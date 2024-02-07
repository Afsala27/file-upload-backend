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
    public class ImgUploadDeleteController : ControllerBase
    {
        private readonly DataContext _context;

        public ImgUploadDeleteController(DataContext context)
        {
            _context = context;
        }
     [HttpDelete]
        public async Task<ActionResult<List<ImgData>>> DeleteImgUpload(int id)
        {
            var upload = await _context.ImgDatas.FindAsync(id);
            try
            {
            if(upload is null)
                return NotFound("Data not found");
            _context.ImgDatas.Remove(upload);
            await _context.SaveChangesAsync();

            return Ok(await _context.ImgDatas.ToListAsync());
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error: "+ex.Message);
            }
        }
    }
}
