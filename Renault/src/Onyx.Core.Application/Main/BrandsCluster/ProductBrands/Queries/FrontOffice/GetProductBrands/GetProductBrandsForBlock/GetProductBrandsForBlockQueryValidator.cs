using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.ProductBrands.Queries.FrontOffice.GetProductBrands.GetProductBrandsForBlock;
public class GetProductBrandsForBlockQueryValidator : AbstractValidator<GetProductBrandsForBlockQuery>
{
    public GetProductBrandsForBlockQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1).WithMessage("محدوده باید بزرگتر یا مساوی یک باشد");
    }
}