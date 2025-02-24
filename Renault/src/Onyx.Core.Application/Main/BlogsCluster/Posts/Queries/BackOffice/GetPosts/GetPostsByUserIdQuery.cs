using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPosts;

public record GetPostsByUserIdQuery(Guid UserId) : IRequest<List<PostDto>>;

public class GetPostsByUserIdQueryHandler : IRequestHandler<GetPostsByUserIdQuery, List<PostDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostsByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostDto>> Handle(GetPostsByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .Where(x => x.AuthorId == request.UserId)
            .OrderBy(x => x.AuthorId)
            .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
