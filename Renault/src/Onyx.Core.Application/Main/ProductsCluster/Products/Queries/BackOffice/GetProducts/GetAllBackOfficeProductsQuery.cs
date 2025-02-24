using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProducts;

public record GetAllBackOfficeProductsQuery : IRequest<FilteredProductDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}


public class GetAllBackOfficeProductsQueryHandler : IRequestHandler<GetAllBackOfficeProductsQuery, FilteredProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBackOfficeProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredProductDto> Handle(GetAllBackOfficeProductsQuery request, CancellationToken cancellationToken)
    {
        var products = _context.Products.OrderBy(c => c.LocalizedName).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            products = products.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            products = products.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await products.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbProducts = await products.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<BackOfficeProductDto>(_mapper.ConfigurationProvider);
        return new FilteredProductDto()
        {
            Products = dbProducts,
            Count = count
        };
    }
}
