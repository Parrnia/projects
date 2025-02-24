using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetOrder.GetOrderForReturnById;
public class GetOrderForReturnByIdQueryValidator : AbstractValidator<GetOrderForReturnByIdQuery>
{
    public GetOrderForReturnByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}