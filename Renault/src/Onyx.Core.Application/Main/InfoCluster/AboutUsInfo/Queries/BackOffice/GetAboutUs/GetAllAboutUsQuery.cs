using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.AboutUsInfo.Queries.BackOffice.GetAboutUs;

public record GetAllAboutUsQuery : IRequest<List<AboutUsDto>>;

public class GetAllAboutUsQueryHandler : IRequestHandler<GetAllAboutUsQuery, List<AboutUsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllAboutUsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AboutUsDto>> Handle(GetAllAboutUsQuery request, CancellationToken cancellationToken)
    {
        return await _context.AboutUsEnumerable
            .ProjectTo<AboutUsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
