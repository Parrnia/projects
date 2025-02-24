using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.GetCustomerTypes;
public record GetAllCustomerTypesQuery : IRequest<List<CustomerTypeDto>>;

public class GetAllCustomerTypesQueryHandler : IRequestHandler<GetAllCustomerTypesQuery, List<CustomerTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCustomerTypesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CustomerTypeDto>> Handle(GetAllCustomerTypesQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.CustomerTypes
            .OrderBy(x => x.CustomerTypeEnum)
            .ProjectTo<CustomerTypeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        return users;
    }
}
