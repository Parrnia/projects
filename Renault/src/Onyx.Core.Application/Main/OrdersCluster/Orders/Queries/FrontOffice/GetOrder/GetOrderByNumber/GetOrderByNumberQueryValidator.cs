using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderByNumber;
public class GetOrderByNumberQueryValidator : AbstractValidator<GetOrderByNumberQuery>
{
    public GetOrderByNumberQueryValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("شماره سفارش اجباریست");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("شماره تماس اجباریست");
    }
}