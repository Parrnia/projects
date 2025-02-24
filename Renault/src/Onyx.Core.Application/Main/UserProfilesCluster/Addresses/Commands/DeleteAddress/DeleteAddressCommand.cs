using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.DeleteAddress;

public record DeleteAddressCommand(int Id, Guid? CustomerId) : IRequest<Unit>;

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Addresses
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Address), request.Id);
        }
        if (request.CustomerId != null && entity.CustomerId != request.CustomerId)
        {
            throw new ForbiddenAccessException("DeleteCommentCommand");
        }
        var customerAddresses = _context.Addresses.Where(e => e.CustomerId == entity.CustomerId && e.Id != request.Id);
        var address = customerAddresses.FirstOrDefaultAsync(cancellationToken).Result;
        if (customerAddresses.All(e => e.Default == false) && entity.CustomerId != null && address != null)
        {
            address.Default = true;
        }
        _context.Addresses.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}