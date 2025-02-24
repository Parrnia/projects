using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice.GetTag;

public record GetTagByIdQuery(int Id) : IRequest<TagDto?>;

public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, TagDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TagDto?> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .ProjectTo<TagDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
