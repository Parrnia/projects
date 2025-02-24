using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CustomerSupportCluster;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.UpdateCustomerTicket;
public record UpdateCustomerTicketCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Subject { get; init; } = null!;
    public string Message { get; init; } = null!;
    public bool IsActive { get; init; }
}

public class UpdateCustomerTicketCommandHandler : IRequestHandler<UpdateCustomerTicketCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateCustomerTicketCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCustomerTicketCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CustomerTickets
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CustomerTicket), request.Id);
        }

        entity.Subject = request.Subject;
        entity.Message = request.Message;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}