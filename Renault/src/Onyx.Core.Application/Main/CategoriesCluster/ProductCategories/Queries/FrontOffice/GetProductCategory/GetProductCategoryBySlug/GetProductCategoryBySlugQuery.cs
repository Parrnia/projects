using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategory.GetProductCategoryBySlug;

public record GetProductCategoryBySlugQuery : IRequest<ProductCategoryBySlugDto?>
{
    public string Slug { get; set; } = null!;
};

public class GetProductCategoryBySlugQueryHandler : IRequestHandler<GetProductCategoryBySlugQuery, ProductCategoryBySlugDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductCategoryBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductCategoryBySlugDto?> Handle(GetProductCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .ProjectTo<ProductCategoryBySlugDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken: cancellationToken);
    }
}
