using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.FrontOffice.GetProductTypesWithPagination;
public record GetProductTypesWithPaginationQuery : IRequest<PaginatedList<ProductTypeWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductTypesWithPaginationQueryHandler : IRequestHandler<GetProductTypesWithPaginationQuery, PaginatedList<ProductTypeWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductTypesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductTypeWithPaginationDto>> Handle(GetProductTypesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypes.AsNoTracking()
            .OrderBy(x => x.Name)
            .ProjectTo<ProductTypeWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
