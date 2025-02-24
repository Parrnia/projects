using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Structure.Color.ProductOptionValueColors.Queries.FrontOffice.GetOptionValueColors.GetByColorId;

public record GetOptionValueColorsByColorIdQuery(int ColorId) : IRequest<List<OptionValueColorByColorIdDto>>;

public class GetAllProductOptionValueColorsByColorIdQueryHandler : IRequestHandler<GetOptionValueColorsByColorIdQuery, List<OptionValueColorByColorIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllProductOptionValueColorsByColorIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OptionValueColorByColorIdDto>> Handle(GetOptionValueColorsByColorIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductOptionValueColors
            .Where(x => x.ProductOptionColorId == request.ColorId)
            .OrderBy(x => x.ProductOptionColorId)
            .ProjectTo<OptionValueColorByColorIdDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}