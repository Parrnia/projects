using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.UpdateProductAttributeType;
public class UpdateProductAttributeTypeCommandValidator : AbstractValidator<UpdateProductAttributeTypeCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public UpdateProductAttributeTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("نام اجباریست")
            .MustAsync(BeUniqueName).WithMessage("نوع ویژگی محصولی با این نام موجود است");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.ProductAttributeTypes
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }

    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}