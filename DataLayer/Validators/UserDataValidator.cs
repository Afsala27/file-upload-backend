using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using FluentValidation;

namespace DataLayer.Validators
{
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