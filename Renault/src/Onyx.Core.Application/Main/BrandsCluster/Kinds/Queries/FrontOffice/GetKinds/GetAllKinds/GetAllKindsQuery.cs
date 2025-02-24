using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKinds.GetAllKinds;
public record GetAllKindsQuery : IRequest<List<AllKindDto>>;

public class GetAllKindsQueryHandler : IRequestHandler<GetAllKindsQuery, List<AllKindDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllKindsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllKindDto>> Handle(GetAllKindsQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.Kinds
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllKindDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        foreach (var allKindDto in res)
        {
            var list1 = allKindDto.Products.Where(c => c.IsActive).ToList();
            var list2 = allKindDto.Vehicles.Where(c => c.IsActive).ToList();
            allKindDto.Products = list1;
            allKindDto.Vehicles = list2;
        }

        return res;
    }
}
