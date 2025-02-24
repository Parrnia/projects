using FluentValidation;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategory;
public class GetProductCategoryBySlugQueryValidator : AbstractValidator<GetProductCategoryBySlugQuery>
{
    public GetProductCategoryBySlugQueryValidator()
    {
        //RuleFor(x => x.Slug)
        //    .NotEmpty().WithMessage("عنوان کوتاه اجباریست");
    }
}