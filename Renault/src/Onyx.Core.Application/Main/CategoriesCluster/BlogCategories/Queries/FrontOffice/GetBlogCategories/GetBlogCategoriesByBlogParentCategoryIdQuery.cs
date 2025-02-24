using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.FrontOffice.GetBlogCategories;

public record GetBlogCategoriesByBlogParentCategoryIdQuery : IRequest<List<BlogCategoryDto>>
{
    public int? BlogParentCategoryId { get; init; }
}

public class GetBlogCategoriesByBlogGroupIdQueryHandler : IRequestHandler<GetBlogCategoriesByBlogParentCategoryIdQuery, List<BlogCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBlogCategoriesByBlogGroupIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BlogCategoryDto>> Handle(GetBlogCategoriesByBlogParentCategoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.BlogCategories
            .Where(x => x.BlogParentCategoryId == request.BlogParentCategoryId)
            .ProjectTo<BlogCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
