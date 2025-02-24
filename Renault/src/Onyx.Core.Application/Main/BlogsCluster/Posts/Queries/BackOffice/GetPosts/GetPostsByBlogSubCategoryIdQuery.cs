using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPosts;

public record GetPostsByBlogSubCategoryIdQuery(int BlogSubCategoryId) : IRequest<List<PostDto>>;


public class GetPostsByBlogSubCategoryIdQueryHandler : IRequestHandler<GetPostsByBlogSubCategoryIdQuery, List<PostDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostsByBlogSubCategoryIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostDto>> Handle(GetPostsByBlogSubCategoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Posts.AsNoTracking()
            .Where(x => x.BlogCategoryId == request.BlogSubCategoryId)
            .OrderBy(x => x.AuthorId)
            .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
