using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.FrontOffice.GetProductType;
public class GetProductTypeByIdQueryValidator : AbstractValidator<GetProductTypeByIdQuery>
{
    public GetProductTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}