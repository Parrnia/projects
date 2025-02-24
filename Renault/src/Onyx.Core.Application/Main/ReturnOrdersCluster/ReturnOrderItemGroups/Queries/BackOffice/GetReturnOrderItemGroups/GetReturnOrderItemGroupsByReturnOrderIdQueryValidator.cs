using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemGroups.Queries.BackOffice.GetReturnOrderItemGroups;
public class GetReturnOrderItemGroupsByReturnOrderIdQueryValidator : AbstractValidator<GetReturnOrderItemGroupsByReturnOrderIdQuery>
{
    public GetReturnOrderItemGroupsByReturnOrderIdQueryValidator()
    {
        RuleFor(x => x.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازگشت اجباریست");
    }
}
