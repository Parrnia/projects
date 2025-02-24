using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.FrontOffice.GetBlogCategory;

public record GetBlogCategoryBySlugQuery : IRequest<BlogCategoryDto?>
{
    public string Slug { get; set; } = null!;
};

public class GetBlogCategoryBySlugQueryHandler : IRequestHandler<GetBlogCategoryBySlugQuery, BlogCategoryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBlogCategoryBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BlogCategoryDto?> Handle(GetBlogCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _context.BlogCategories
            .ProjectTo<BlogCategoryDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken: cancellationToken);
    }
}
