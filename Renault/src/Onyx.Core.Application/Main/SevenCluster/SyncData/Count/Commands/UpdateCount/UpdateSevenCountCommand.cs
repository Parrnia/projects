using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.SevenCluster.SyncData.Count.Commands.UpdateCount;
public record UpdateSevenCountsCommand : IRequest<Unit>
{
    public List<UpdateSevenCountCommand> CountCommands { get; init; } = new List<UpdateSevenCountCommand>();
}
public record UpdateSevenCountCommand : IRequest<Unit>
{
    public Guid Related7SoftProductId { get; init; }
    public double Count { get; init; }
}

public class UpdateSevenCountsCommandHandler : IRequestHandler<UpdateSevenCountsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateSevenCountsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSevenCountsCommand request, CancellationToken cancellationToken)
    {
        foreach (var command in request.CountCommands)
        {
            var product = await _context.Products
                .Include(c => c.AttributeOptions)
                .SingleOrDefaultAsync(c => c.Related7SoftProductId == command.Related7SoftProductId, cancellationToken);

            var attributeOption = product?.AttributeOptions.SingleOrDefault();

            attributeOption?.SetTotalCount(command.Count);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}