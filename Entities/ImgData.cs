using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace FileUploadApplication.Entities
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

    public class ImgDataValidator : AbstractValidator<ImgData>
    {
    public ImgDataValidator()
    {
        RuleFor(x => x.ImgDataId).NotNull();
        //RuleFor(x => x.ImageData).NotEmpty();
        RuleFor(x => x.Caption).NotEmpty();
        //RuleFor(x => x.Password).Length(8).NotEmpty();
    }
    }

}