using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderById;
public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}