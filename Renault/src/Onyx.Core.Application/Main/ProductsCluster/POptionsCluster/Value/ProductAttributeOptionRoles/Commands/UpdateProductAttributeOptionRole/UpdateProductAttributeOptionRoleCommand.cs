using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Commands.UpdateProductAttributeOptionRole;
public record UpdateProductAttributeOptionRoleCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public double MinimumStockToDisplayProductForThisCustomerTypeEnum { get; init; }
    public double MainMaxOrderQty { get; init; }
    public double MainMinOrderQty { get; init; }
    public int CustomerTypeEnumId { get; init; }
    public double DiscountPercent { get; init; }
    public int ProductAttributeOptionId { get; init; }
}

public class UpdateProductAttributeOptionRoleCommandHandler : IRequestHandler<UpdateProductAttributeOptionRoleCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductAttributeOptionRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductAttributeOptionRoleCommand request, CancellationToken cancellationToken)
    {
        var productAttributeOption =
            await _context.ProductAttributeOptions
                .SingleOrDefaultAsync(c => c.Id == request.ProductAttributeOptionId, cancellationToken);
        if (productAttributeOption == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOption), request.ProductAttributeOptionId);
        }

        var entity = await _context.ProductAttributeOptionRoles
            .FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException(nameof(ProductAttributeOptionRole), request.Id);
        }

        entity.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(request.MinimumStockToDisplayProductForThisCustomerTypeEnum);
        entity.CustomerTypeEnum =(CustomerTypeEnum) request.CustomerTypeEnumId;
        entity.DiscountPercent = request.DiscountPercent;
        entity.SetMainMaxOrderQty(request.MainMaxOrderQty);
        entity.SetMainMinOrderQty(request.MainMinOrderQty);



        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}