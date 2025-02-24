using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.CreateCustomer;
public record CreateCustomerCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid? Avatar { get; init; }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateCustomerCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
        if (_context.Customers.Any(c => c.Id == request.Id))
        {
            return request.Id;
        }
        var entity = new Customer()
        {
            Id = request.Id,
            Avatar = request.Avatar,
        };

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

        _context.Customers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
