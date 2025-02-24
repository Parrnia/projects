using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Badges.Commands.UpdateBadge;
public record UpdateBadgeCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Value { get; init; } = null!;
    public bool IsActive { get; init; }
    public IList<int> ProductAttributeOptionIds { get; set; } = new List<int>();
}

public class UpdateBadgeCommandHandler : IRequestHandler<UpdateBadgeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateBadgeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBadgeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Badges
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Badge), request.Id);
        }
        var productAttributeOptions = new List<ProductAttributeOption>();
        foreach (var productAttributeOptionId in request.ProductAttributeOptionIds)
        {
            var productAttributeOption = await _context.ProductAttributeOptions.FindAsync(productAttributeOptionId, cancellationToken) ?? throw new NotFoundException(nameof(ProductAttributeOption), productAttributeOptionId);
            productAttributeOptions.Add(productAttributeOption);
        }
        entity.Value = request.Value;
        entity.ProductAttributeOptions = productAttributeOptions;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}