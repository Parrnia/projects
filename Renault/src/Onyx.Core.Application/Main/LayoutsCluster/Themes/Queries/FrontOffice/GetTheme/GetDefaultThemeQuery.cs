using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Queries.FrontOffice.GetTheme;
public record GetDefaultThemeQuery : IRequest<ThemeDto?>;

public class GetDefaultThemeQueryHandler : IRequestHandler<GetDefaultThemeQuery, ThemeDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDefaultThemeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ThemeDto?> Handle(GetDefaultThemeQuery request, CancellationToken cancellationToken)
    {
        return await _context.Themes
            .ProjectTo<ThemeDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.IsDefault == true, cancellationToken: cancellationToken);
    }
}
