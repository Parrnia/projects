using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.CustomerSupportCluster;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.CreateCustomerTicket;
public record CreateCustomerTicketCommand : IRequest<int>
{
    public string Subject { get; init; } = null!;
    public string Message { get; init; } = null!;
    public Guid CustomerId { get; set; }
    public string CustomerPhoneNumber { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
}

public class CreateCustomerTicketCommandHandler : IRequestHandler<CreateCustomerTicketCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerTicketCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCustomerTicketCommand request, CancellationToken cancellationToken)
    {
        var entity = new CustomerTicket()
        {
            Subject = request.Subject,
            Message = request.Message,
            CustomerPhoneNumber = request.CustomerPhoneNumber,
            CustomerName = request.CustomerName,
            CustomerId = request.CustomerId,
            IsActive = true,
        };

        _context.CustomerTickets.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
