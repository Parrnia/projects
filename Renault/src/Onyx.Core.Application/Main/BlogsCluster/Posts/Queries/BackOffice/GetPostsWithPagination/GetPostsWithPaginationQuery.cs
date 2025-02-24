using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPostsWithPagination;
public record GetPostsWithPaginationQuery : IRequest<PaginatedList<PostDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPostsWithPaginationQueryHandler : IRequestHandler<GetPostsWithPaginationQuery, PaginatedList<PostDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PostDto>> Handle(GetPostsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .OrderBy(x => x.AuthorId)
            .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
