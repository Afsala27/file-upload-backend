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
    public class LoginApiController : ControllerBase
    {
        private readonly DataContext _context;

        public LoginApiController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserData logindata)
        {
            var user = await _context.UserDatas.FirstOrDefaultAsync(u => u.Email == logindata.Email);
            try
            {
                if (user == null || user.Password != logindata.Password)
                    return Unauthorized("Invalid email or password");
                return Ok(new { LoggedUserId= user.UserId, Message="Succesfully logged with id " + user.UserId} ); // need to send this into image upload api
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error : "+ ex.Message);
            }

        }
    }
}