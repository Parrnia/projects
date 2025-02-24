using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamilies;
public record GetAllFamiliesQuery : IRequest<List<FamilyDto>>;

public class GetAllFamiliesQueryHandler : IRequestHandler<GetAllFamiliesQuery, List<FamilyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllFamiliesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyDto>> Handle(GetAllFamiliesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Families.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<FamilyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
