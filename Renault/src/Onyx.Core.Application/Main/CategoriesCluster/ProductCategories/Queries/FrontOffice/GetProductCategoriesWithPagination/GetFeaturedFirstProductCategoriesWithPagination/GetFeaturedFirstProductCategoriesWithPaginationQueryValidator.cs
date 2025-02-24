using FluentValidation;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.FrontOffice.GetProductCategoriesWithPagination.GetFeaturedFirstProductCategoriesWithPagination;
public class GetFeaturedFirstProductCategoriesWithPaginationQueryValidator : AbstractValidator<GetFeaturedFirstProductCategoriesWithPaginationQuery>
{
    public GetFeaturedFirstProductCategoriesWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}