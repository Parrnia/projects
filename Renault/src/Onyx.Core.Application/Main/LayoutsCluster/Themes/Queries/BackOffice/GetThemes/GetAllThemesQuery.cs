using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice.GetThemes;
public record GetAllThemesQuery : IRequest<List<ThemeDto>>;

public class GetAllThemesQueryHandler : IRequestHandler<GetAllThemesQuery, List<ThemeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllThemesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ThemeDto>> Handle(GetAllThemesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Themes
            .OrderBy(x => x.Title)
            .ProjectTo<ThemeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
