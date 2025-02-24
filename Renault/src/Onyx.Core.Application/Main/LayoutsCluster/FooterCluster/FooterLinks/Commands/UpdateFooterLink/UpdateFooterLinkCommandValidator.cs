using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Commands.UpdateFooterLink;
public class UpdateFooterLinkCommandValidator : AbstractValidator<UpdateFooterLinkCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateFooterLinkCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان اجباریست");
        RuleFor(v => v.Url)
            .NotEmpty().WithMessage("آدرس url اجباریست");
        RuleFor(v => v.FooterLinkContainerId)
            .NotEmpty().WithMessage("شناسه دسته بندی لینک های فوتر اجباریست");
    }
}