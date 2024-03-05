using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.DTOs.User;
using DataLayer.Entities;
using Google.Apis.Drive.v3.Data;

namespace DataLayer.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this UserData userModel)
        {
            return new UserDto
            {
                UserId = userModel.UserId,
                Name = userModel.Name,
                Email = userModel.Email,
               // Password = ""
            };

        }
    }
}