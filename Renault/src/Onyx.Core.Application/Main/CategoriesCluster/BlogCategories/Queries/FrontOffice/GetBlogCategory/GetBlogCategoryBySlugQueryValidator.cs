using FluentValidation;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.FrontOffice.GetBlogCategory;
public class GetBlogCategoryBySlugQueryValidator : AbstractValidator<GetBlogCategoryBySlugQuery>
{
    public GetBlogCategoryBySlugQueryValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("عنوان کوتاه اجباریست");
    }
}