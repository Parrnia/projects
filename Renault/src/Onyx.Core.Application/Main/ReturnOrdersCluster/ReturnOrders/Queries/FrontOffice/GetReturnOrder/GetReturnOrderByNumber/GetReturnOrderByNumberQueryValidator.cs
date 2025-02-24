using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrder.GetReturnOrderByNumber;
public class GetReturnOrderByNumberQueryValidator : AbstractValidator<GetReturnOrderByNumberQuery>
{
    public GetReturnOrderByNumberQueryValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("شماره سفارش اجباریست");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("شماره تماس اجباریست");
    }
}