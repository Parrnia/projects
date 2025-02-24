using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Queries.FrontOffice.GetAboutUs;

public record GetAboutUsQuery : IRequest<AboutUsDto?>;

public class GetAboutUsQueryHandler : IRequestHandler<GetAboutUsQuery, AboutUsDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAboutUsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AboutUsDto?> Handle(GetAboutUsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AboutUsEnumerable
            .ProjectTo<AboutUsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(a => a.IsDefault,cancellationToken: cancellationToken);
    }
}
