using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CustomerSupportCluster;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.DeleteCustomerTicket;

public record DeleteCustomerTicketCommand(int Id, Guid? CustomerId) : IRequest<Unit>;

public class DeleteCustomerTicketCommandHandler : IRequestHandler<DeleteCustomerTicketCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerTicketCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCustomerTicketCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CustomerTickets
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CustomerTicket), request.Id);
        }
        if (request.CustomerId != null && entity.CustomerId != request.CustomerId)
        {
            throw new ForbiddenAccessException("DeleteCustomerTicketCommand");
        }
        _context.CustomerTickets.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}