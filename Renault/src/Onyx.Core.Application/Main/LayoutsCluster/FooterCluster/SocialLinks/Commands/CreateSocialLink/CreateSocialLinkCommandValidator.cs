using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.CreateSocialLink;
public class CreateSocialLinkCommandValidator : AbstractValidator<CreateSocialLinkCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateSocialLinkCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Url)
            .NotEmpty().WithMessage("آدرس url اجباریست");
        RuleFor(v => v.Icon)
            .NotEmpty().WithMessage("شناسه آیکون اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");

    }
}
