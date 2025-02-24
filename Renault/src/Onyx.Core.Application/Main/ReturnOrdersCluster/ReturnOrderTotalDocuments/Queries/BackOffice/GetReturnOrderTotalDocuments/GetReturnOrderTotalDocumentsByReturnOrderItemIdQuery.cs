using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Queries.BackOffice.GetReturnOrderTotalDocuments;

public record GetReturnOrderTotalDocumentsByReturnOrderTotalIdQuery(int ReturnOrderTotalId) : IRequest<List<ReturnOrderTotalDocumentDto>>;


public class GetReturnOrderTotalDocumentsByReturnOrderTotalIdQueryHandler : IRequestHandler<GetReturnOrderTotalDocumentsByReturnOrderTotalIdQuery, List<ReturnOrderTotalDocumentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReturnOrderTotalDocumentsByReturnOrderTotalIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderTotalDocumentDto>> Handle(GetReturnOrderTotalDocumentsByReturnOrderTotalIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrderTotalDocuments
            .Where(i => i.ReturnOrderTotalId == request.ReturnOrderTotalId)
            .OrderBy(x => x.Description)
            .ProjectTo<ReturnOrderTotalDocumentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
