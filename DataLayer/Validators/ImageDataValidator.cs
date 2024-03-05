using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using FluentValidation;

namespace DataLayer.Validators
{
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