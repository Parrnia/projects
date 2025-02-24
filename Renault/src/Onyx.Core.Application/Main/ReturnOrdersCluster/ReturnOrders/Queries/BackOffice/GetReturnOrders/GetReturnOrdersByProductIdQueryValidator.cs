using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrders;
public class GetReturnOrdersByProductIdQueryValidator : AbstractValidator<GetReturnOrdersByProductIdQuery>
{
    public GetReturnOrdersByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}
