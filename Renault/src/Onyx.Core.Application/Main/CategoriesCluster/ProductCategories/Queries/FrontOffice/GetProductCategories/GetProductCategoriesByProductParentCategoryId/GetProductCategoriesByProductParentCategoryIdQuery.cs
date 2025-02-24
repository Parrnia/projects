using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategories.GetProductCategoriesByProductParentCategoryId;

public record GetProductCategoriesByProductParentCategoryIdQuery : IRequest<List<ProductCategoryByProductParentCategoryIdDto>>
{
    public int? ProductParentCategoryId { get; init; }
}

public class GetProductCategoriesByProductParentCategoryIdQueryHandler : IRequestHandler<GetProductCategoriesByProductParentCategoryIdQuery, List<ProductCategoryByProductParentCategoryIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductCategoriesByProductParentCategoryIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductCategoryByProductParentCategoryIdDto>> Handle(GetProductCategoriesByProductParentCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var categoryDtos = await _context.ProductCategories
            .Where(x => x.ProductParentCategoryId == request.ProductParentCategoryId && x.IsActive)
            .ProjectTo<ProductCategoryByProductParentCategoryIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var categoryDto in categoryDtos)
        {
            var list = categoryDto.ProductChildrenCategories?.Where(c => c.IsActive).ToList();
            categoryDto.ProductChildrenCategories = list;
        }

        return categoryDtos;
    }
}
