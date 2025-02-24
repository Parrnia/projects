using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.BackOffice.GetReturnOrderItemDocuments;

public record GetReturnOrderItemDocumentsByReturnOrderItemIdQuery(int ReturnOrderItemId) : IRequest<List<ReturnOrderItemDocumentDto>>;


public class GetReturnOrderItemDocumentsByReturnOrderItemIdQueryHandler : IRequestHandler<GetReturnOrderItemDocumentsByReturnOrderItemIdQuery, List<ReturnOrderItemDocumentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderItemDocumentsByReturnOrderItemIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderItemDocumentDto>> Handle(GetReturnOrderItemDocumentsByReturnOrderItemIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.ReturnOrderItemDocuments
            .Where(i => i.ReturnOrderItemId == request.ReturnOrderItemId)
            .OrderBy(x => x.Description)
            .ProjectTo<ReturnOrderItemDocumentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}
