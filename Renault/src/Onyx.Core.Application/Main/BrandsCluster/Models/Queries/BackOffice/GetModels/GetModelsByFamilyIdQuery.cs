using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice.GetModels;

public record GetModelsByFamilyIdQuery(int FamilyId) : IRequest<List<ModelDto>>;

public class GetModelsByFamilyIdQueryHandler : IRequestHandler<GetModelsByFamilyIdQuery, List<ModelDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModelsByFamilyIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ModelDto>> Handle(GetModelsByFamilyIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Models.AsNoTracking()
            .Where(x => x.FamilyId == request.FamilyId)
            .OrderBy(x => x.Name)
            .ProjectTo<ModelDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
