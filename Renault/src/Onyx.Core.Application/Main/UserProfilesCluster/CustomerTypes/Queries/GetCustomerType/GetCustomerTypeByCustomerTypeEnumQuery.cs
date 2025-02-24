using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Queries.GetCustomerType;

public record GetCustomerTypeByCustomerTypeEnumQuery(CustomerTypeEnum CustomerTypeEnum) : IRequest<CustomerTypeDto?>;

public class GetCustomerTypeByCustomerTypeEnumQueryHandler : IRequestHandler<GetCustomerTypeByCustomerTypeEnumQuery, CustomerTypeDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerTypeByCustomerTypeEnumQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerTypeDto?> Handle(GetCustomerTypeByCustomerTypeEnumQuery request, CancellationToken cancellationToken)
    {
        var role = await _context.CustomerTypes
            .ProjectTo<CustomerTypeDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.CustomerTypeEnum == request.CustomerTypeEnum, cancellationToken: cancellationToken);
        return role;
    }
}
