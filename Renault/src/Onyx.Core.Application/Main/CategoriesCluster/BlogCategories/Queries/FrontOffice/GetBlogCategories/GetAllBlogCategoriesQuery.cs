using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.FrontOffice.GetBlogCategories;
public record GetAllBlogCategoriesQuery : IRequest<List<BlogCategoryDto>>;

public class GetAllBlogCategoriesQueryHandler : IRequestHandler<GetAllBlogCategoriesQuery, List<BlogCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllBlogCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BlogCategoryDto>> Handle(GetAllBlogCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoryDtos = await _context.BlogCategories
            .Where(c => c.IsActive)
            .OrderBy(x => x.Name)
            .ProjectTo<BlogCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        foreach (var categoryDto in categoryDtos)
        {
            var list = categoryDto.BlogChildrenCategories?.Where(c => c.IsActive).ToList();
            categoryDto.BlogChildrenCategories = list;
        }
        return categoryDtos;
    }
}
