using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Prices.Commands.UpdatePrice;
public record UpdatePriceCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public decimal MainPrice { get; init; }
    public int ProductAttributeOptionId { get; init; }
}

public class UpdatePriceCommandHandler : IRequestHandler<UpdatePriceCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdatePriceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Prices
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Price), request.Id);
        }
        entity.Date = request.Date;
        entity.MainPrice = request.MainPrice;
        entity.ProductAttributeOptionId = request.ProductAttributeOptionId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}