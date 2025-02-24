using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Badges.Commands.CreateBadge;
public record CreateBadgeCommand : IRequest<int>
{
    public string Value { get; init; } = null!;
    public bool IsActive { get; init; }
    public IList<int> ProductAttributeOptionIds { get; set; } = new List<int>();
}

public class CreateBadgeCommandHandler : IRequestHandler<CreateBadgeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateBadgeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateBadgeCommand request, CancellationToken cancellationToken)
    {
        var productAttributeOptions = new List<ProductAttributeOption>();
        foreach (var productAttributeOptionId in request.ProductAttributeOptionIds)
        {
            var productAttributeOption = await _context.ProductAttributeOptions.FindAsync(productAttributeOptionId, cancellationToken) ?? throw new NotFoundException(nameof(ProductAttributeOption), productAttributeOptionId);
            productAttributeOptions.Add(productAttributeOption);
        }
        var entity = new Badge()
        {
            Value = request.Value,
            ProductAttributeOptions = productAttributeOptions,
            IsActive = request.IsActive
        };

        _context.Badges.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
