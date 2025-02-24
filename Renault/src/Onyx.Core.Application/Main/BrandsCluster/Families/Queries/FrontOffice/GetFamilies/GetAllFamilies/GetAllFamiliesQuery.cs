using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamilies.GetAllFamilies;
public record GetAllFamiliesQuery : IRequest<List<AllFamilyDto>>;

public class GetAllFamiliesQueryHandler : IRequestHandler<GetAllFamiliesQuery, List<AllFamilyDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllFamiliesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllFamilyDto>> Handle(GetAllFamiliesQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.Families
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllFamilyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var familyDto in res)
        {
            var list = familyDto.Models.Where(c => c.IsActive).ToList();
            familyDto.Models = list;
        } 
        return res;
    }
}
