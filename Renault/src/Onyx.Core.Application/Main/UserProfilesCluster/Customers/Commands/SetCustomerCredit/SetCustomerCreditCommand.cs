using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.UserProfilesCluster;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.SetCustomerCredit;
public record SetCustomerCreditCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public decimal Credit { get; init; }
    public string ModifierUserName { get; set; } = null!;
    public Guid ModifierUserId { get; set; }

}

public class SetCustomerCreditCommandHandler : IRequestHandler<SetCustomerCreditCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public SetCustomerCreditCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SetCustomerCreditCommand request, CancellationToken cancellationToken)
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

        var successFul = entity.SetCredit(request.Credit, request.ModifierUserId, request.ModifierUserName);
        if (!successFul)
        {
            throw new OrderException("مقدار اعتبار نمی تواند کمتر از صفر یا بزرگتر از سقف اعتبار باشد");
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}