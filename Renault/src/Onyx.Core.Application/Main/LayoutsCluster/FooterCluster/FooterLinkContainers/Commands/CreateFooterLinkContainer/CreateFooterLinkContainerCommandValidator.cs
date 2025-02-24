using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Commands.CreateFooterLinkContainer;
public class CreateFooterLinkContainerCommandValidator : AbstractValidator<CreateFooterLinkContainerCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateFooterLinkContainerCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Header)
            .NotEmpty().WithMessage("هدر اجباریست");
        RuleFor(v => v.Order)
            .NotEmpty().WithMessage("ترتیب اجباریست")
            .GreaterThanOrEqualTo(1).WithMessage("ترتیب یاید بزرگتر از صفر باشد");
    }
}
