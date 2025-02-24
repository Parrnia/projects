using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Commands.CreateAboutUs;
public class CreateAboutUsCommandValidator : AbstractValidator<CreateAboutUsCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateAboutUsCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("محتوای عنوان اجباریست");
        RuleFor(v => v.TextContent)
            .NotEmpty().WithMessage("محتوای متنی اجباریست");
        RuleFor(v => v.ImageContent)
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است")
            .NotEmpty().WithMessage("شناسه تصویر اجباریست");

    }
}
