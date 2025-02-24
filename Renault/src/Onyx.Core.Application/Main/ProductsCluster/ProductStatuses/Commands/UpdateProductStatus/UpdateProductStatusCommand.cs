using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Commands.UpdateProductStatus;
public record UpdateProductStatusCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int Code { get; init; }
    public string Name { get; init; } = null!;
    public string LocalizedName { get; init; } = null!;
}

public class UpdateProductStatusCommandHandler : IRequestHandler<UpdateProductStatusCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductStatuses
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductStatus), request.Id);
        }

        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.LocalizedName = request.LocalizedName;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}