using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Mvc;

namespace DataLayer.Entities
{
    public class UserData
    {
        [Key]
        public int UserId { get; set; }

        public required string? Name { get; set; }

          public required string? Email { get; set; }
        public required string? Password { get; set; }


        //adding navigation property

        public List<ImgData>? ImgDataList ;

    }

}