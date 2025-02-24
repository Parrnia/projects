using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.SetCustomerMaxCredit;
public record SetCustomerMaxCreditCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public decimal MaxCredit { get; init; }
    public string ModifierUserName { get; set; } = null!;
    public Guid ModifierUserId { get; set; }

}

public class SetCustomerMaxCreditCommandHandler : IRequestHandler<SetCustomerMaxCreditCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public SetCustomerMaxCreditCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SetCustomerMaxCreditCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .Include(c => c.MaxCredits)
            .Include(c => c.Credits)
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            var recoverEntity = new Customer()
            {
                Id = request.Id
            };

            _context.Customers.Add(recoverEntity);
            await _context.SaveChangesAsync(cancellationToken);
            entity = recoverEntity;
        }

        var successFul = entity.SetMaxCredit(request.MaxCredit, request.ModifierUserId, request.ModifierUserName);
        if (!successFul)
        {
            throw new OrderException("مقدار حداکثر اعتبار نمی تواند کمتر از صفر باشد");
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}