using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByProductCategorySlug;
public class GetProductsByProductCategorySlugQueryValidator : AbstractValidator<GetProductsByProductCategorySlugQuery>
{
    public GetProductsByProductCategorySlugQueryValidator()
    {
        RuleFor(x => x.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
    }
}
