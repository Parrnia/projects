using FluentValidation;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategory.GetProductCategoryById;
public class GetProductCategoryByIdQueryValidator : AbstractValidator<GetProductCategoryByIdQuery>
{
    public GetProductCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}