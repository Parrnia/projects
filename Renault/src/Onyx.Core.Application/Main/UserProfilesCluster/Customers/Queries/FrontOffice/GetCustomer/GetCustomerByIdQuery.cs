using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Queries.FrontOffice.GetCustomer;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            var recoverEntity = new Customer()
            {
                Id = request.Id
            };

            _context.Customers.Add(recoverEntity);
            await _context.SaveChangesAsync(cancellationToken);
            entity = await _context.Customers
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        }

        return entity;
    }
}
