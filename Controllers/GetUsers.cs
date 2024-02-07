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
    public class GetUserController : ControllerBase
    {
        private readonly DataContext _context;

        public GetUserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserData>> GetUsers(int id)
        {
            try
            {
                var upload1 = await _context.UserDatas.FindAsync(id);
                if(upload1 is null)
                    return NotFound("Specific Data not found");
                return Ok(upload1);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error: "+ex.Message);
            }
        }
    }
}