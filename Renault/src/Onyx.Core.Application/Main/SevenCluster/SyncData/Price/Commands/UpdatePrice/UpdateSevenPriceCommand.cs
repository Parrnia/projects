using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.SevenCluster.SyncData.Price.Commands.UpdatePrice;
public record UpdateSevenPricesCommand : IRequest<Unit>
{
    public List<UpdateSevenPriceCommand> PriceCommands { get; init; } = new List<UpdateSevenPriceCommand>();
}
public record UpdateSevenPriceCommand : IRequest<Unit>
{
    public Guid Related7SoftProductId { get; init; }
    public decimal Price { get; init; }
}

public class UpdateSevenPricesCommandHandler : IRequestHandler<UpdateSevenPricesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateSevenPricesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSevenPricesCommand request, CancellationToken cancellationToken)
    {
        foreach (var command in request.PriceCommands)
        {
            var product = await _context.Products
                .Include(c => c.AttributeOptions)
                .SingleOrDefaultAsync(c => c.Related7SoftProductId == command.Related7SoftProductId, cancellationToken);

            var attributeOption = product?.AttributeOptions.SingleOrDefault();

            attributeOption?.SetPrice(command.Price);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}