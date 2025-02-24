using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.CreateFooterLink;
public class CreateFooterLinkCommandValidator : AbstractValidator<CreateFooterLinkCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateFooterLinkCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان اجباریست");
        RuleFor(v => v.Url)
            .NotEmpty().WithMessage("آدرس url اجباریست");
        RuleFor(v => v.FooterLinkContainerId)
            .NotEmpty().WithMessage("شناسه دسته بندی لینک های فوتر اجباریست");
    }
}
