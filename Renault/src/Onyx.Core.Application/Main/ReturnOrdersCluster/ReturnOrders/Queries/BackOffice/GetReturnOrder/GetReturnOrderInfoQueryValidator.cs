using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrder;
public class GetReturnOrderInfoQueryValidator : AbstractValidator<GetReturnOrderInfoQuery>
{
    public GetReturnOrderInfoQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}