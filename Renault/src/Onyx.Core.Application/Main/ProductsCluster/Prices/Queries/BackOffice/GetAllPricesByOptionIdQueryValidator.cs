using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Prices.Queries.BackOffice;
public class GetAllPricesByOptionIdQueryValidator : AbstractValidator<GetAllPricesByOptionIdQuery>
{
    public GetAllPricesByOptionIdQueryValidator()
    {
        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("شناسه آپشن اجباریست");
    }
}