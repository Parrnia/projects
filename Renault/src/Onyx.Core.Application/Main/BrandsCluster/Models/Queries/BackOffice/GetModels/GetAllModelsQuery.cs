using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice.GetModels;
public record GetAllModelsQuery : IRequest<List<ModelDto>>;

public class GetAllModelsQueryHandler : IRequestHandler<GetAllModelsQuery, List<ModelDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllModelsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ModelDto>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
    {
        var models = await _context.Models
            .OrderBy(x => x.Name)
            .ProjectTo<ModelDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return models;
    }
}
