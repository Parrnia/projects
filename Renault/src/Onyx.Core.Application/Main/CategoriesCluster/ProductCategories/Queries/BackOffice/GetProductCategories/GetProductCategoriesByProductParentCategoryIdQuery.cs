using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategories;

public record GetProductCategoriesByProductParentCategoryIdQuery : IRequest<List<ProductCategoryDto>>
{
    public int? ProductParentCategoryId { get; init; }
}

public class GetProductCategoriesByProductParentCategoryIdQueryHandler : IRequestHandler<GetProductCategoriesByProductParentCategoryIdQuery, List<ProductCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductCategoriesByProductParentCategoryIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductCategoryDto>> Handle(GetProductCategoriesByProductParentCategoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductCategories
            .Where(x => x.ProductParentCategoryId == request.ProductParentCategoryId)
            .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
