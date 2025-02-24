using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrders;
public class GetOrdersByProductIdQueryValidator : AbstractValidator<GetOrdersByProductIdQuery>
{
    public GetOrdersByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}
