using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategory;

public record GetProductCategoryBySlugQuery: IRequest<ProductCategoryDto?>
{
    public string Slug { get; set; } = null!;
};

public class GetProductCategoryBySlugQueryHandler : IRequestHandler<GetProductCategoryBySlugQuery, ProductCategoryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductCategoryBySlugQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductCategoryDto?> Handle(GetProductCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken: cancellationToken);
    }
}
