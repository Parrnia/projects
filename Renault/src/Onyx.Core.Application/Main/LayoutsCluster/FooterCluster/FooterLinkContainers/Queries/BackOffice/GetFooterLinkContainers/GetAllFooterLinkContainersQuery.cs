using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.BackOffice.GetFooterLinkContainers;
public record GetAllFooterLinkContainersQuery : IRequest<List<FooterLinkContainerDto>>;

public class GetAllFooterLinkContainersQueryHandler : IRequestHandler<GetAllFooterLinkContainersQuery, List<FooterLinkContainerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllFooterLinkContainersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FooterLinkContainerDto>> Handle(GetAllFooterLinkContainersQuery request, CancellationToken cancellationToken)
    {
        var brands = await _context.FooterLinkContainers
            .OrderBy(x => x.Order)
            .ProjectTo<FooterLinkContainerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return brands;
    }
}
