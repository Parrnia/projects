using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Commands.UpdateFooterLinkContainer;
public class UpdateFooterLinkContainerCommandValidator : AbstractValidator<UpdateFooterLinkContainerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateFooterLinkContainerCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Header)
            .NotEmpty().WithMessage("هدر اجباریست");
        RuleFor(v => v.Order)
            .NotEmpty().WithMessage("ترتیب اجباریست")
            .GreaterThanOrEqualTo(1).WithMessage("ترتیب یاید بزرگتر از صفر باشد");
    }
}