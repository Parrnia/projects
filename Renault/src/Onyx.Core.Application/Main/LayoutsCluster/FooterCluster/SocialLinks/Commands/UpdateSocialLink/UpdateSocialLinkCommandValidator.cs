using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.SocialLinks.Commands.UpdateSocialLink;
public class UpdateSocialLinkCommandValidator : AbstractValidator<UpdateSocialLinkCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSocialLinkCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Url)
            .NotEmpty().WithMessage("آدرس url اجباریست");
        RuleFor(v => v.Icon)
            .NotEmpty().WithMessage("شناسه آیکون اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }

}