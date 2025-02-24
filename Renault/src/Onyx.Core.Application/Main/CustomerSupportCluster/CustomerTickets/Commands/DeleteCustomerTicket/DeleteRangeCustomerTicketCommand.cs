using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CustomerSupportCluster;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.DeleteCustomerTicket;

public class DeleteRangeCustomerTicketCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeCustomerTicketCommandHandler : IRequestHandler<DeleteRangeCustomerTicketCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeCustomerTicketCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeCustomerTicketCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.CustomerTickets
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(CustomerTicket), id);
            }

            _context.CustomerTickets.Remove(entity);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
