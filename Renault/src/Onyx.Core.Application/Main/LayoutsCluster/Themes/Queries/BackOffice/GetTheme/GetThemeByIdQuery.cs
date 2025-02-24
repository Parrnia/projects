using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice.GetTheme;

public record GetThemeByIdQuery(int Id) : IRequest<ThemeDto?>;

public class GetThemeByIdQueryHandler : IRequestHandler<GetThemeByIdQuery, ThemeDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetThemeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ThemeDto?> Handle(GetThemeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Themes
            .ProjectTo<ThemeDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
