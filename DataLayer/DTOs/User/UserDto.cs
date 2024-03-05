using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.DTOs.User
{
    public class UserDto
    {
        public int UserId { get; set; }

        public required string? Name { get; set; }

        public required string? Email { get; set; }

       // public required string? Password { get; set; }
    }
}