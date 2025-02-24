using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModel.GetModelById;

public record GetModelByIdQuery(int Id) : IRequest<ModelByIdDto?>;

public class GetModelByIdQueryHandler : IRequestHandler<GetModelByIdQuery, ModelByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetModelByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ModelByIdDto?> Handle(GetModelByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Models
            .ProjectTo<ModelByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
