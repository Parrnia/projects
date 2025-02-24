using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.OrderPayments.BackOffice.Queries.GetOrderPayments;
public class GetOrderPaymentsByOrderIdQueryValidator : AbstractValidator<GetOrderPaymentsByOrderIdQuery>
{
    public GetOrderPaymentsByOrderIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("شناسه سفارش اجباریست");
    }
}
