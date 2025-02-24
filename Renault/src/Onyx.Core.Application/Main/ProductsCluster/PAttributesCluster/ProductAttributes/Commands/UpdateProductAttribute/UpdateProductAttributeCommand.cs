using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;

namespace Onyx.Application.Main.ProductsCluster.PAttributesCluster.ProductAttributes.Commands.UpdateProductAttribute;
public record UpdateProductAttributeCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public bool Featured { get; init; }
    public string ValueName { get; set; } = null!;
}

public class UpdateProductAttributeCommandHandler : IRequestHandler<UpdateProductAttributeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductAttributeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttribute), request.Id);
        }

        entity.Featured = request.Featured;
        entity.ValueName = request.ValueName;
        entity.ValueSlug = request.ValueName.ToLower().Replace(' ', '-');


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}