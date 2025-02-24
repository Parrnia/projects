using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModels.GetModelsByFamilyId;

public record GetModelsByFamilyIdQuery(int FamilyId) : IRequest<List<ModelByFamilyIdDto>>;

public class GetModelsByFamilyIdQueryHandler : IRequestHandler<GetModelsByFamilyIdQuery, List<ModelByFamilyIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModelsByFamilyIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ModelByFamilyIdDto>> Handle(GetModelsByFamilyIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Models
            .Where(x => x.FamilyId == request.FamilyId && x.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<ModelByFamilyIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
