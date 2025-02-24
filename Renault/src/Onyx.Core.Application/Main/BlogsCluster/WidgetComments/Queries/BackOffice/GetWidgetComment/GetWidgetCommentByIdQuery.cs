using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice.GetWidgetComment;

public record GetWidgetCommentByIdQuery(int Id) : IRequest<WidgetCommentDto?>;

public class GetWidgetCommentByIdQueryHandler : IRequestHandler<GetWidgetCommentByIdQuery, WidgetCommentDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWidgetCommentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WidgetCommentDto?> Handle(GetWidgetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.WidgetComments
            .ProjectTo<WidgetCommentDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
