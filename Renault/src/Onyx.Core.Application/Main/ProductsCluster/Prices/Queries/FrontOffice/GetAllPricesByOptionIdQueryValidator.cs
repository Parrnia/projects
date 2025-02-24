using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Prices.Queries.FrontOffice;
public class GetAllPricesByOptionIdQueryValidator : AbstractValidator<BackOffice.GetAllPricesByOptionIdQuery>
{
    public GetAllPricesByOptionIdQueryValidator()
    {
        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("شناسه آپشن اجباریست");
    }
}