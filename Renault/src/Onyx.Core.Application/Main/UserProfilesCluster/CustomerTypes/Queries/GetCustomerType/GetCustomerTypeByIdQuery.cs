using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.GetCustomerType;

public record GetCustomerTypeByIdQuery(int Id) : IRequest<CustomerTypeDto?>;

public class GetCustomerTypeByIdQueryHandler : IRequestHandler<GetCustomerTypeByIdQuery, CustomerTypeDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerTypeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerTypeDto?> Handle(GetCustomerTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _context.CustomerTypes
            .ProjectTo<CustomerTypeDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        return role;
    }
}
