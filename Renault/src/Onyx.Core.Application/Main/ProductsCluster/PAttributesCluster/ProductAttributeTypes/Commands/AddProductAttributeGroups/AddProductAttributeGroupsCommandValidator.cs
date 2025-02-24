using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributeTypes.Commands.AddProductAttributeGroups;
public class AddProductAttributeGroupsCommandValidator : AbstractValidator<AddProductAttributeGroupsCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public AddProductAttributeGroupsCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه ویژگی محصول اجباریست");
        RuleFor(v => v.AttributeGroupIds)
            .MustAsync(BeUniqueName).WithMessage("یکی از محصولات، یک ویژگی محصول با این نام دارد");
    }
    public async Task<bool> BeUniqueName(IList<int> attributeGroupIds, CancellationToken cancellationToken)
    {
        var attributeGroups = new List<ProductTypeAttributeGroup>();
        foreach (var attributeGroupId in attributeGroupIds)
        {
            var attributeGroup = await _context.ProductTypeAttributeGroups
                .Include(c => c.Attributes)
                .SingleOrDefaultAsync(c => c.Id == attributeGroupId, cancellationToken) ?? throw new NotFoundException(nameof(ProductTypeAttributeGroup), attributeGroupId);
            attributeGroups.Add(attributeGroup);
        }
        var entity = await _context.ProductAttributeTypes
        .Include(c => c.AttributeGroups)
        .ThenInclude(c => c.Attributes)
        .Include(c => c.Products)
        .ThenInclude(c => c.Attributes)
        .SingleOrDefaultAsync(c => c.Id == _id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeType), _id);
        }
        foreach (var product in entity.Products)
        {
            foreach (var attribute in attributeGroups.SelectMany(c => c.Attributes))
            {
                if (product.Attributes.Any(c => c.Name == attribute.Value))
                {
                    return false;
                }
            }
        }

        return true;
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}
