using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrder;
public class GetReturnOrderByIdQueryValidator : AbstractValidator<GetReturnOrderByIdQuery>
{
    public GetReturnOrderByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}