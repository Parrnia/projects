using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Commands.UpdateTheme;
public record UpdateThemeCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Title { get; init; } = null!;
    public string BtnPrimaryColor { get; init; } = null!;
    public string BtnPrimaryHoverColor { get; init; } = null!;
    public string BtnSecondaryColor { get; init; } = null!;
    public string BtnSecondaryHoverColor { get; init; } = null!;
    public string ThemeColor { get; init; } = null!;
    public string HeaderAndFooterColor { get; init; } = null!;
    public bool IsDefault { get; init; }
}

public class UpdateThemeCommandHandler : IRequestHandler<UpdateThemeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateThemeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateThemeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Themes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Theme), request.Id);
        }

        entity.Title = request.Title;
        entity.BtnPrimaryColor = request.BtnPrimaryColor;
        entity.BtnPrimaryHoverColor = request.BtnPrimaryHoverColor;
        entity.BtnSecondaryColor = request.BtnSecondaryColor;
        entity.BtnSecondaryHoverColor = request.BtnSecondaryHoverColor;
        entity.ThemeColor = request.ThemeColor;
        entity.HeaderAndFooterColor = request.HeaderAndFooterColor;
        entity.IsDefault = request.IsDefault;


        var themes = await _context.Themes.ToListAsync(cancellationToken);
        if (request.IsDefault)
        {
            themes?.ForEach(d => d.IsDefault = false);
        }
        if (themes != null && themes.All(e => e.IsDefault == false))
        {
            entity.IsDefault = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
