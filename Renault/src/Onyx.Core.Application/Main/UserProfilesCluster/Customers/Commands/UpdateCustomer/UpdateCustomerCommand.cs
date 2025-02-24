using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.UpdateCustomer;
public record UpdateCustomerCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid? Avatar { get; init; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateCustomerCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            var recoverEntity = new Customer()
            {
                Id = request.Id
            };

            _context.Customers.Add(recoverEntity);
            await _context.SaveChangesAsync(cancellationToken);
            entity = recoverEntity;
        }


        if (entity.Avatar != request.Avatar)
        {
            if (entity.Avatar != null)
            {
                await _fileStore.RemoveStoredFile((Guid)entity.Avatar, cancellationToken);
            }

            if (request.Avatar != null)
            {
                await _fileStore.SaveStoredFile(
                    (Guid)request.Avatar,
                    FileCategory.Customer.ToString(),
                    FileCategory.Customer,
                    null,
                    false,
                    cancellationToken);
            }
            entity.Avatar = request.Avatar;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}