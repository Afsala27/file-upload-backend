using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.DTOs.File
{
    public class VieoDto
    {
        [Key]
        public int ImgDataId { get; set; }
        public required string ? VideoData { get; set; }
        public required string? DriveFileId { get; set; }
        public required string? Caption { get; set; }
        public string? Comment{ get; set; }

        [Required]
        public string? Id { get; set; }
    }
}