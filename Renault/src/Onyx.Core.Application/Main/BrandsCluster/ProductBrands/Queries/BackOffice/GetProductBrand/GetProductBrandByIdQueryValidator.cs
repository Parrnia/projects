using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.BackOffice.GetProductBrand;
public class GetProductBrandByIdQueryValidator : AbstractValidator<GetProductBrandByIdQuery>
{
    public GetProductBrandByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}