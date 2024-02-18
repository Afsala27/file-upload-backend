using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using FileUploadApplication.Entities;
using FileUploadApplication.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
//using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;

namespace FileUploadApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistrationController : ControllerBase
    {
        private IValidator<UserData> _validator;
        private readonly DataContext _context;

        public UserRegistrationController(DataContext context, IValidator<UserData> validator)
        {
            _validator = validator;
            _context = context;
        }


        // [HttpGet]
        // //[Route("api/[controller]")]
        // public async Task<ActionResult<List<FirstApi>>> GetAllUpload()
        // {
        //     var upload = await _context.ApiData.ToListAsync();
        //     return Ok(upload);
        // }

        [HttpPost]
        public async Task<ActionResult<List<UserData>>> AddUpload(UserData demodata)
        {
            UserDataValidator validator = new UserDataValidator();
            ValidationResult result = validator.Validate(demodata);

            //await _validator.ValidateAsync(demodata);
            try
            {
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                var existinguser = await _context.UserDatas.FirstOrDefaultAsync(u => u.Email == demodata.Email);
                if (existinguser != null)
                {
                    return Conflict("User with the provided email already exists.");
                }

                _context.UserDatas.Add(demodata);
                await _context.SaveChangesAsync();

                return Ok("Registration Succesfull");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}