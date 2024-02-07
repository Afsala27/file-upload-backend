using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadApplication.Entities
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

    public class UserDataValidator : AbstractValidator<UserData>
    {
    public UserDataValidator()
    {
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.Name).Length(0, 15).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).Length(8).NotEmpty()
        .Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]")
        .Matches("[^A-Za-z0-9]").WithMessage("Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 symbol.");
    }
    }

}