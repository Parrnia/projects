using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.FooterCluster.FooterLinks.Queries.BackOffice.GetFooterLink;

public record GetFooterLinkByIdQuery(int Id) : IRequest<FooterLinkDto?>;

public class GetFooterLinkByIdQueryHandler : IRequestHandler<GetFooterLinkByIdQuery, FooterLinkDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFooterLinkByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FooterLinkDto?> Handle(GetFooterLinkByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.FooterLinks
            .ProjectTo<FooterLinkDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
