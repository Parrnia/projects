using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.CreateProductTypeAttributeGroup;
public class CreateProductTypeAttributeGroupCommandValidator : AbstractValidator<CreateProductTypeAttributeGroupCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateProductTypeAttributeGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست")
            .MustAsync(BeUniqueName).WithMessage("گروه ویژگی محصولی با این نام موجود است");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductTypeAttributeGroups
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
