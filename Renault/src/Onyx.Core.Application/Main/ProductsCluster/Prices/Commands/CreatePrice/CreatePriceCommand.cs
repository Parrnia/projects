using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;

namespace Onyx.Application.Main.ProductsCluster.Prices.Commands.CreatePrice;
public record CreatePriceCommand : IRequest<int>
{
    public DateTime Date { get; init; }
    public decimal MainPrice { get; init; }
    public int ProductAttributeOptionId { get; init; }
}

public class CreatePriceCommandHandler : IRequestHandler<CreatePriceCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePriceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePriceCommand request, CancellationToken cancellationToken)
    {
        var entity = new Price()
        {
            Date = request.Date,
            MainPrice = request.MainPrice,
            ProductAttributeOptionId = request.ProductAttributeOptionId
        };

        _context.Prices.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
