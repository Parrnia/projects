using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrder.GetReturnOrderById;
public class GetReturnOrderByIdQueryValidator : AbstractValidator<GetReturnOrderByIdQuery>
{
    public GetReturnOrderByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}