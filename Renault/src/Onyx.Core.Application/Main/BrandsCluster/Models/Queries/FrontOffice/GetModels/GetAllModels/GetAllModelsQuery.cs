using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModels.GetAllModels;
public record GetAllModelsQuery : IRequest<List<AllModelDto>>;

public class GetAllModelsQueryHandler : IRequestHandler<GetAllModelsQuery, List<AllModelDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllModelsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllModelDto>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
    {
        var res = await _context.Models
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<AllModelDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var modelDto in res)
        {
            var list = modelDto.Kinds.Where(c => c.IsActive).ToList();
            modelDto.Kinds = list;
        }
        return res;
    }
}
