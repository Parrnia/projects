using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Commands.CreateCustomerType;
public record CreateCustomerTypeCommand : IRequest<int>
{
    public int CustomerTypeEnum { get; init; }
    public double DiscountPercent { get; init; }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateCustomerTypeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCustomerTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.UserProfilesCluster.CustomerType()
        {
            CustomerTypeEnum =(CustomerTypeEnum) request.CustomerTypeEnum,
            DiscountPercent = request.DiscountPercent
        };
   
        _context.CustomerTypes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
