using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductTypeAttributeGroups.Commands.UpdateProductTypeAttributeGroup;
public record UpdateProductTypeAttributeGroupCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
}

public class UpdateProductTypeAttributeGroupCommandHandler : IRequestHandler<UpdateProductTypeAttributeGroupCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductTypeAttributeGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductTypeAttributeGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypeAttributeGroups
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductTypeAttributeGroup), request.Id);
        }

        entity.Name = request.Name;
        entity.Slug = request.Name.ToLower().Replace(' ', '-');

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}