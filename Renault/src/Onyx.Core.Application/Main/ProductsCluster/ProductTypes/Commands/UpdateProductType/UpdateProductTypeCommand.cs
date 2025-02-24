using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Commands.UpdateProductType;
public record UpdateProductTypeCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int Code { get; init; }
    public string Name { get; init; } = null!;
    public string LocalizedName { get; init; } = null!;
}


public class UpdateProductTypeCommandHandler : IRequestHandler<UpdateProductTypeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ProductTypes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductType), request.Id);
        }

        entity.Code = request.Code;
        entity.Name = request.Name;
        entity.LocalizedName = request.LocalizedName;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}