using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrder;
public class GetOrderInfoQueryValidator : AbstractValidator<GetOrderInfoQuery>
{
    public GetOrderInfoQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}