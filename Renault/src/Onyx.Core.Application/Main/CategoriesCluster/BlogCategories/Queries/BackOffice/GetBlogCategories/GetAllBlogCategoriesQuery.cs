using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice.GetBlogCategories;
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
        var brands = await _context.BlogCategories
            .OrderBy(x => x.Name)
            .ProjectTo<BlogCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
