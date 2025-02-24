using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.UpdateProductTypeAttributeGroup;
public class UpdateProductTypeAttributeGroupCommandValidator : AbstractValidator<UpdateProductTypeAttributeGroupCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public UpdateProductTypeAttributeGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست")
            .MustAsync(BeUniqueName).WithMessage("چیزی با این نام لاتین موجود است");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductTypeAttributeGroups
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }

    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}