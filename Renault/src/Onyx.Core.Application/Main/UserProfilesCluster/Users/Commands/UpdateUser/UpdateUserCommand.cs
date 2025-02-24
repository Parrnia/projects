using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.Users.Commands.UpdateUser;
public record UpdateUserCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public Guid? Avatar { get; init; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateUserCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            var recoverEntity = new User()
            {
                Id = request.Id
            };

            _context.Users.Add(recoverEntity);
            await _context.SaveChangesAsync(cancellationToken);
            entity = recoverEntity;
        }

        entity.Id = request.Id;

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
                    FileCategory.ProductBrand.ToString(),
                    FileCategory.ProductBrand,
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