using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.UpdateAboutUs;
public class UpdateAboutUsCommandValidator : AbstractValidator<UpdateAboutUsCommand>
{
    public UpdateAboutUsCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("محتوای عنوان اجباریست");
        RuleFor(v => v.TextContent)
            .NotEmpty().WithMessage("محتوای متنی اجباریست");
        RuleFor(v => v.ImageContent)
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است")
            .NotEmpty().WithMessage("شناسه تصویر اجباریست");

    }
}