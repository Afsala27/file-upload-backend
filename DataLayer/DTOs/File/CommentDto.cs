using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.DTOs.File
{
    public class CommentDto
    {
        [Key]
        public int ImgDataId { get; set; }

        public required string? Caption { get; set; }

        public string? Comment{ get; set; }
    }
}