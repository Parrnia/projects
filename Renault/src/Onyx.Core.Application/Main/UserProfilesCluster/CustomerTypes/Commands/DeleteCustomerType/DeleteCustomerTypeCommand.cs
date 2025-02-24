using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Commands.DeleteCustomerType;

public record DeleteCustomerTypeCommand(int Id) : IRequest<Unit>;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteCustomerTypeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCustomerTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CustomerTypes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CustomerType), request.Id);
        }
        
        _context.CustomerTypes.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}