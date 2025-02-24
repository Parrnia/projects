using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Commands.UpdateCustomerType;
public record UpdateCustomerTypeCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int CustomerTypeEnum { get; init; }
    public double DiscountPercent { get; init; }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateCustomerTypeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCustomerTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CustomerTypes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.UserProfilesCluster.CustomerType), request.Id);
        }
        
        entity.CustomerTypeEnum = (CustomerTypeEnum)request.CustomerTypeEnum;
        entity.DiscountPercent = request.DiscountPercent;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}