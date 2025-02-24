using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.LayoutsCluster;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Commands.CreateTheme;
public record CreateThemeCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public string BtnPrimaryColor { get; init; } = null!;
    public string BtnPrimaryHoverColor { get; init; } = null!;
    public string BtnSecondaryColor { get; init; } = null!;
    public string BtnSecondaryHoverColor { get; init; } = null!;
    public string ThemeColor { get; init; } = null!;
    public string HeaderAndFooterColor { get; init; } = null!;
    public bool IsDefault { get; init; }
    
}

public class CreateThemeCommandHandler : IRequestHandler<CreateThemeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateThemeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateThemeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Theme()
        {
            Title = request.Title,
            BtnPrimaryColor = request.BtnPrimaryColor,
            BtnPrimaryHoverColor = request.BtnPrimaryHoverColor,
            BtnSecondaryColor = request.BtnSecondaryColor,
            BtnSecondaryHoverColor = request.BtnSecondaryHoverColor,
            ThemeColor = request.ThemeColor,
            HeaderAndFooterColor = request.HeaderAndFooterColor,
            IsDefault = request.IsDefault
        };


        var themes = await _context.Themes.ToListAsync(cancellationToken);
        if (request.IsDefault)
        {
            themes?.ForEach(d => d.IsDefault = false);
        }
        if (themes != null && themes.All(e => e.IsDefault == false))
        {
            entity.IsDefault = true;
        }

        _context.Themes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
