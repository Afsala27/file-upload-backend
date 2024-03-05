using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.DTOs.File
{
    public class ImageDto
    {
        [Key]
        public int ImgDataId { get; set; }
        public required string ? ImageData { get; set; }
        public required string? DriveFileId { get; set; }
        public required string? Caption { get; set; }
        public int UserId { get; set; }
    }
}