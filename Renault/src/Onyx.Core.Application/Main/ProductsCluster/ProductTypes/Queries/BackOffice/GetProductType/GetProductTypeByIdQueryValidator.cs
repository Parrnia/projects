using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.ProductTypes.Queries.BackOffice.GetProductType;
public class GetProductTypeByIdQueryValidator : AbstractValidator<GetProductTypeByIdQuery>
{
    public GetProductTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}