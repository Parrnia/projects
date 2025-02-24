using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ReturnOrdersCluster.OptionValues.Queries.BackOffice.GetOptionValues;

public record GetOptionValuesByReturnOrderItemGroupIdQuery(int ReturnOrderItemGroupId) : IRequest<List<ReturnOrderItemGroupProductAttributeOptionValueDto>>;


public class GetOptionValuesByReturnOrderItemGroupIdQueryHandler : IRequestHandler<GetOptionValuesByReturnOrderItemGroupIdQuery, List<ReturnOrderItemGroupProductAttributeOptionValueDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOptionValuesByReturnOrderItemGroupIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ReturnOrderItemGroupProductAttributeOptionValueDto>> Handle(GetOptionValuesByReturnOrderItemGroupIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ReturnOrderItemGroupProductAttributeOptionValues
            .Where(i => i.ReturnOrderItemGroupId == request.ReturnOrderItemGroupId)
            .OrderBy(x => x.Name)
            .ProjectTo<ReturnOrderItemGroupProductAttributeOptionValueDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
