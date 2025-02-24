using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinkContainers.Queries.BackOffice.GetFooterLinkContainer;

public record GetFooterLinkContainerByIdQuery(int Id) : IRequest<FooterLinkContainerDto?>;

public class GetFooterLinkContainerByIdQueryHandler : IRequestHandler<GetFooterLinkContainerByIdQuery, FooterLinkContainerDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFooterLinkContainerByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FooterLinkContainerDto?> Handle(GetFooterLinkContainerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.FooterLinkContainers
            .ProjectTo<FooterLinkContainerDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
