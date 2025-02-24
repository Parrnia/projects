using FluentValidation;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategory.GetProductCategoryBySlug;
public class GetProductCategoryBySlugQueryValidator : AbstractValidator<GetProductCategoryBySlugQuery>
{
    public GetProductCategoryBySlugQueryValidator()
    {
        //RuleFor(x => x.Slug)
        //    .NotEmpty().WithMessage("عنوان کوتاه اجباریست");
    }
}