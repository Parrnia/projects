using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Commands.DeleteTheme;
public record DeleteThemeCommand(int Id) : IRequest<Unit>;

public class DeleteThemeCommandHandler : IRequestHandler<DeleteThemeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteThemeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteThemeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Themes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Theme), request.Id);
        }

        _context.Themes.Remove(entity);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}