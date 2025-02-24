using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.FrontOffice.GetBlogCategory;

public record GetBlogCategoryByIdQuery(int Id) : IRequest<BlogCategoryDto?>;

public class GetBlogCategoryByIdQueryHandler : IRequestHandler<GetBlogCategoryByIdQuery, BlogCategoryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBlogCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BlogCategoryDto?> Handle(GetBlogCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.BlogCategories
            .ProjectTo<BlogCategoryDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
