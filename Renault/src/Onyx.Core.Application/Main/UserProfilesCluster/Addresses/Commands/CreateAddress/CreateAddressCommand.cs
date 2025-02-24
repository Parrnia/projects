using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.CreateAddress;
public record CreateAddressCommand : IRequest<int>
{
    public string Title { get; init; } = null!;
    public string? Company { get; init; }
    public int CountryId { get; init; }
    public string AddressDetails1 { get; init; } = null!;
    public string? AddressDetails2 { get; init; }
    public string City { get; init; } = null!;
    public string State { get; init; } = null!;
    public string Postcode { get; init; } = null!;
    public bool Default { get; init; }
    public Guid CustomerId { get; set; }
}

public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.Include(c => c.Addresses).SingleOrDefaultAsync(
            e => e.Id == request.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new NotFoundException(nameof(Customer), request.CustomerId);
        }
        var entity = new Address()
        {
            Title = request.Title,
            Company = request.Company,
            CountryId = request.CountryId,
            AddressDetails1 = request.AddressDetails1,
            AddressDetails2 = request.AddressDetails2,
            City = request.City,
            State = request.State,
            Postcode = request.Postcode,
            Default = request.Default,
            CustomerId = request.CustomerId,
        };
        var customerAddresses = customer?.Addresses.ToList();
        if (request.Default)
        {
             customerAddresses?.ForEach(d => d.Default = false);
        }
        if (customerAddresses != null && customerAddresses.All(e => e.Default == false))
        {
            entity.Default = true;
        }
        _context.Addresses.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
