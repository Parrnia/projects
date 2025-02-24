using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.Users.Commands.CreateUser;
public record CreateUserCommand : IRequest<Guid>
{
    public Guid Id { get; init; }
    public Guid? Avatar { get; init; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateUserCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (_context.Users.Any(c => c.Id == request.Id))
        {
            return request.Id;
        }

        var entity = new User()
        {
            Id = request.Id,
            Avatar = request.Avatar
        };


        if (request.Avatar != null)
        {
            await _fileStore.SaveStoredFile(
                (Guid)request.Avatar,
                FileCategory.User.ToString(),
                FileCategory.User,
                null,
                false,
                cancellationToken);
        }

        _context.Users.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
