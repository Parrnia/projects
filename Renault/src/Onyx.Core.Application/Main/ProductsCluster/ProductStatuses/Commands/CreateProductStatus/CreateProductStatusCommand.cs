using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Commands.CreateProductStatus;
public record CreateProductStatusCommand : IRequest<int>
{
    public int Code { get; init; }
    public string Name { get; init; } = null!;
    public string LocalizedName { get; init; } = null!;
}

public class CreateProductStatusCommandHandler : IRequestHandler<CreateProductStatusCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductStatus()
        {
            Code = request.Code,
            Name = request.Name,
            LocalizedName = request.LocalizedName
        };

        _context.ProductStatuses.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
