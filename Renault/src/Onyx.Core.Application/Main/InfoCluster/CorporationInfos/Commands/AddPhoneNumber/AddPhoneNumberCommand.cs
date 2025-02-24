using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.AddPhoneNumber;
public record AddPhoneNumberCommand : IRequest<int>
{
    public int CorporationInfoId { get; init; }
    public string PhoneNumber { get; init; } = null!;
}

public class AddPhoneNumberCommandHandler : IRequestHandler<AddPhoneNumberCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddPhoneNumberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(AddPhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CorporationInfos
            .SingleOrDefaultAsync(c => c.Id == request.CorporationInfoId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CorporationInfo), request.CorporationInfoId);
        }

        var phoneNumbers = entity.PhoneNumbers.ToList();
        phoneNumbers.Add(request.PhoneNumber);

        entity.PhoneNumbers = phoneNumbers;

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
