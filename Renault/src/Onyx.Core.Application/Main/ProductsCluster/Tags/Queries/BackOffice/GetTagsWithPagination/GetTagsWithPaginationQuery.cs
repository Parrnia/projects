using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice.GetTagsWithPagination;
public record GetTagsWithPaginationQuery : IRequest<FilteredTagDto>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortColumn { get; init; } = null!;
    public string? SortDirection { get; init; } = null!;
    public string? SearchTerm { get; init; } = null!;
}

public class GetTagsWithPaginationQueryHandler : IRequestHandler<GetTagsWithPaginationQuery, FilteredTagDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTagsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FilteredTagDto> Handle(GetTagsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var tags = _context.Tags.OrderBy(c => c.Created).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            tags = tags.ApplySearch(request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortColumn) && !string.IsNullOrWhiteSpace(request.SortDirection))
        {
            tags = tags.ApplySorting(request.SortColumn, request.SortDirection);
        }

        var count = await tags.CountAsync(cancellationToken);
        var skip = (request.PageNumber - 1) * request.PageSize;
        var dbTags = await tags.Skip(skip).Take(request.PageSize)
            .ProjectToListAsync<TagDto>(_mapper.ConfigurationProvider);
        return new FilteredTagDto()
        {
            Tags = dbTags,
            Count = count
        };
    }
}
