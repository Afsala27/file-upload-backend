using System.ComponentModel.DataAnnotations;


namespace DataLayer.Entities
{
    public class ImgData
    {
        [Key]
        public int ImgDataId { get; set; }

        public required string ? ImageData { get; set; }
        public required string ? VideoData { get; set; }

        public required string? Caption { get; set; }

        public string? Comment {get; set;}= string.Empty;

        public required string? DriveFileId { get; set; }

        // adding forien key to the img data
        public int UserId { get; set; }
        public UserData? UserData ;

    }

}