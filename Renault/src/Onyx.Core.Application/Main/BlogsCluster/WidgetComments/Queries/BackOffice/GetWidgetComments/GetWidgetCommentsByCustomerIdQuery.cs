using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice.GetWidgetComments;

public record GetWidgetCommentsByCustomerIdQuery(Guid CustomerId) : IRequest<List<WidgetCommentDto>>;

public class GetWidgetCommentsByCustomerIdQueryHandler : IRequestHandler<GetWidgetCommentsByCustomerIdQuery, List<WidgetCommentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWidgetCommentsByCustomerIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WidgetCommentDto>> Handle(GetWidgetCommentsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var widgetComments = await _context.WidgetComments
            .Where(b => b.AuthorId == request.CustomerId)
            .ProjectTo<WidgetCommentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return widgetComments;
    }
}
