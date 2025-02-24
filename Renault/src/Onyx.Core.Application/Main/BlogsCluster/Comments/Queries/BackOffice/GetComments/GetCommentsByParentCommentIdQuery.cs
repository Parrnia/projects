using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComments;

public record GetCommentsByParentCommentIdQuery(int ParentCommentId) : IRequest<List<CommentDto>>;

public class GetCommentsByParentCommentIdQueryHandler : IRequestHandler<GetCommentsByParentCommentIdQuery, List<CommentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCommentsByParentCommentIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> Handle(GetCommentsByParentCommentIdQuery request, CancellationToken cancellationToken)
    {
        var comments = await _context.Comments.AsNoTracking()
            //.Where(x => x.ParentCommentId == request.ParentCommentId)
            .OrderBy(x => x.AuthorId)
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return comments;
    }
}
