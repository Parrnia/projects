using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Queries.BackOffice.GetReturnOrderItems;
public class GetReturnOrderItemsByReturnOrderItemGroupIdQueryValidator : AbstractValidator<GetReturnOrderItemsByReturnOrderItemGroupIdQuery>
{
    public GetReturnOrderItemsByReturnOrderItemGroupIdQueryValidator()
    {
        RuleFor(x => x.ReturnOrderItemGroupId)
            .NotEmpty().WithMessage("شناسه گروه آیتم سفارش بازگشت اجباریست");
    }
}
