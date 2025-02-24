using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.BackOffice.GetModel;

public record GetModelByIdQuery(int Id) : IRequest<ModelDto?>;

public class GetModelByIdQueryHandler : IRequestHandler<GetModelByIdQuery, ModelDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModelByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ModelDto?> Handle(GetModelByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Models
            .ProjectTo<ModelDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
