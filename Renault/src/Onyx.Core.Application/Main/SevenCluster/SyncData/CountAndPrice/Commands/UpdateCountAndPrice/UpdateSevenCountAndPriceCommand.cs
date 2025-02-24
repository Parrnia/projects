using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.SevenCluster.SyncData.CountAndPrice.Commands.UpdateCountAndPrice;
public record UpdateSevenCountsAndPricesCommand : IRequest<Unit>
{
    public List<UpdateSevenCountAndPriceCommand> CountAndPriceCommands { get; init; } = new List<UpdateSevenCountAndPriceCommand>();
}
public record UpdateSevenCountAndPriceCommand : IRequest<Unit>
{
    public Guid Related7SoftProductId { get; init; }
    public double Count { get; init; }
    public decimal Price { get; init; }
}

public class UpdateSevenCountsAndPricesCommandHandler : IRequestHandler<UpdateSevenCountsAndPricesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateSevenCountsAndPricesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSevenCountsAndPricesCommand request, CancellationToken cancellationToken)
    {
        foreach (var command in request.CountAndPriceCommands)
        {
            var product = await _context.Products
                .Include(c => c.AttributeOptions)
                .SingleOrDefaultAsync(c => c.Related7SoftProductId == command.Related7SoftProductId, cancellationToken);

            var attributeOption = product?.AttributeOptions.SingleOrDefault();

            attributeOption?.SetTotalCount(command.Count);
            attributeOption?.SetPrice(command.Price);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}