using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Commands.UpdateProductAttributeOptionValue;
public record UpdateProductAttributeOptionValueCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Value { get; init; } = null!;
}

public class UpdateProductAttributeOptionValueCommandHandler : IRequestHandler<UpdateProductAttributeOptionValueCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductAttributeOptionValueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductAttributeOptionValueCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductAttributeOptionValues
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOptionValue), request.Id);
        }

        entity.Name = request.Name;
        entity.Value = request.Value;



        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}