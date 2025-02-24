using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComments;

public record GetCommentsByCustomerIdQuery(Guid CustomerId) : IRequest<List<CommentDto>>;

public class GetCommentsByCustomerIdQueryHandler : IRequestHandler<GetCommentsByCustomerIdQuery, List<CommentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCommentsByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> Handle(GetCommentsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var comments = await _context.Comments
            .Where(x => x.AuthorId == request.CustomerId)
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        return comments;
    }
}
