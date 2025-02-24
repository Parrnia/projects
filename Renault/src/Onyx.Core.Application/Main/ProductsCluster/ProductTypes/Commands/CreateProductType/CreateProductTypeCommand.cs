using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Commands.CreateProductType;
public record CreateProductTypeCommand : IRequest<int>
{
    public int Code { get; init; }
    public string Name { get; init; } = null!;
    public string LocalizedName { get; init; } = null!;
}

public class CreateProductTypeCommandHandler : IRequestHandler<CreateProductTypeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new ProductType()
        {
            Code = request.Code,
            Name = request.Name,
            LocalizedName = request.LocalizedName,
        };

        _context.ProductTypes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
