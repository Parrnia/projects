using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.FrontOffice.GetTags;
public record GetAllTagsQuery : IRequest<List<AllTagDto>>;

public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, List<AllTagDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllTagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Where(c => c.IsActive)
            .ProjectTo<AllTagDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
