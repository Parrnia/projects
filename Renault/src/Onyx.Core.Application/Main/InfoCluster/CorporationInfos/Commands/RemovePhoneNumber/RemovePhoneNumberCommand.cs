using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.RemovePhoneNumber;
public record RemovePhoneNumberCommand : IRequest<int>
{
    public int CorporationInfoId { get; init; }
    public List<string> PhoneNumbers { get; init; } = new List<string>();
}

public class RemovePhoneNumberCommandHandler : IRequestHandler<RemovePhoneNumberCommand, int>
{
    private readonly IApplicationDbContext _context;

    public RemovePhoneNumberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(RemovePhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .SingleOrDefaultAsync(c => c.Id == request.CorporationInfoId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.CorporationInfoId);
        }

        var phoneNumbers = entity.PhoneNumbers.ToList();

        foreach (var phoneNumber in request.PhoneNumbers)
        {
            phoneNumbers.Remove(phoneNumber);
        }

        entity.PhoneNumbers = phoneNumbers;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
