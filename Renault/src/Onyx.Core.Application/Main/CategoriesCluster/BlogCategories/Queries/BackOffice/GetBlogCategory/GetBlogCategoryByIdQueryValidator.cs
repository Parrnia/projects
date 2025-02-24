using FluentValidation;

namespace Onyx.Application.Main.CategoriesCluster.BlogCategories.Queries.BackOffice.GetBlogCategory;
public class GetBlogCategoryByIdQueryValidator : AbstractValidator<GetBlogCategoryByIdQuery>
{
    public GetBlogCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}