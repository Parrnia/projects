using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.OrdersCluster;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.UpdateOrder;
public record UpdateOrderCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string AddressDetails1 { get; init; } = null!;
    public string AddressDetails2 { get; init; } = null!;
    public string Postcode { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .Include(c => c.OrderAddress)
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }


        entity.OrderAddress.AddressDetails1 = request.AddressDetails1;
        entity.OrderAddress.AddressDetails2 = request.AddressDetails2;
        entity.OrderAddress.Postcode = request.Postcode;
        entity.PhoneNumber = request.PhoneNumber;
        entity.CustomerFirstName = request.FirstName;
        entity.CustomerLastName = request.LastName;


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
