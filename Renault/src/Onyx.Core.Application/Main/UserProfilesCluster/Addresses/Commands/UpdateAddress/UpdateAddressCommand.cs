using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.UpdateAddress;
public record UpdateAddressCommand : IRequest<Unit>
{
    public int Id { get; init; }
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

public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateAddressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Addresses
            .Include(c => c.Customer)
            .ThenInclude(c => c.Addresses)
            .SingleOrDefaultAsync(c=> c.Id == request.Id , cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(Address), request.Id);
        }


        entity.Title = request.Title;
        entity.Company = request.Company;
        entity.CountryId = request.CountryId;
        entity.AddressDetails1 = request.AddressDetails1;
        entity.AddressDetails2 = request.AddressDetails2;
        entity.City = request.City;
        entity.State = request.State;
        entity.Postcode = request.Postcode;
        entity.Default = request.Default;

        var customerAddresses = entity.Customer?.Addresses.Where(e => e.Id != request.Id).ToList();

        if (request.Default)
        {
            customerAddresses?.ForEach(d => d.Default = false);
        }

        if (customerAddresses != null && customerAddresses.All(e => e.Default == false))
        {
            entity.Default = true;
        }
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}