using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Commands.CreateProductAttributeOptionRole;
public record CreateProductAttributeOptionRoleCommand : IRequest<int>
{
    public double MinimumStockToDisplayProductForThisCustomerTypeEnum { get; init; }
    public double MainMaxOrderQty { get; init; }
    public double MainMinOrderQty { get; init; }
    public int CustomerTypeEnumId { get; init; }
    public double DiscountPercent { get; init; }
    public int ProductAttributeOptionId { get; init; }
}

public class CreateProductAttributeOptionRoleCommandHandler : IRequestHandler<CreateProductAttributeOptionRoleCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductAttributeOptionRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductAttributeOptionRoleCommand request, CancellationToken cancellationToken)
    {
        var productAttributeOption =
            await _context.ProductAttributeOptions
                .SingleOrDefaultAsync(c => c.Id == request.ProductAttributeOptionId,cancellationToken);
        if (productAttributeOption == null)
        {
            
            throw new NotFoundException(nameof(ProductAttributeOption), request.ProductAttributeOptionId);
        }

        var entity = new ProductAttributeOptionRole();
        entity.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(request.MinimumStockToDisplayProductForThisCustomerTypeEnum);
        entity.CustomerTypeEnum = (CustomerTypeEnum) request.CustomerTypeEnumId;
        entity.DiscountPercent = request.DiscountPercent;
        entity.ProductAttributeOptionId = request.ProductAttributeOptionId;
        entity.SetMainMaxOrderQty(request.MainMaxOrderQty);
        entity.SetMainMinOrderQty(request.MainMinOrderQty);


        _context.ProductAttributeOptionRoles.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
