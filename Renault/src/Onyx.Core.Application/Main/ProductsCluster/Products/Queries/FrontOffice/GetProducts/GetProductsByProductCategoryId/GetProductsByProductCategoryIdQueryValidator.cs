using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProducts.GetProductsByProductCategoryId;
public class GetProductsByProductCategoryIdQueryValidator : AbstractValidator<GetProductsByProductCategoryIdQuery>
{
    public GetProductsByProductCategoryIdQueryValidator()
    {
        RuleFor(x => x.ProductCategoryId)
            .NotEmpty().WithMessage("شناسه دسته بندی محصول اجباریست");
        RuleFor(x => x.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
    }
}
