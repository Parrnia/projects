using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.RemoveProductAttributeGroups;
public class RemoveProductAttributeGroupsCommandValidator : AbstractValidator<RemoveProductAttributeGroupsCommand>
{
    private readonly IApplicationDbContext _context;
    public RemoveProductAttributeGroupsCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه ویژگی محصول اجباریست");
    }
}
