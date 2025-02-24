using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.FrontOffice.GetTag;

public record GetTagByIdQuery(int Id) : IRequest<TagByIdDto?>;

public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, TagByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TagByIdDto?> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .ProjectTo<TagByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
